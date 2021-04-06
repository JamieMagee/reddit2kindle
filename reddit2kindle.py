#!/usr/bin/env python3

import os

from flask import Flask, request, jsonify
from flask.templating import render_template

import forms
import util

app = Flask(__name__)
app.secret_key = os.urandom(24)
forms.csrf.init_app(app)


@app.route('/')
def index():
    post = forms.Submission()
    subreddit = forms.Subreddit()
    return render_template('index.html', post=post, subreddit=subreddit)


@app.route('/thread', methods=['POST'])
def thread():
    #print(dir(util))
    if util.validate_request_post(request.form) is not None:
        return jsonify(type='danger', text=util.validate_request_post(request.form))

    try:
        submission = util.r.submission(url=request.form['submission'])
    except:
        return jsonify(type='danger', text='That wasn\'t a reddit link, was it?')

    
    if not submission.url.startswith('https://www.reddit.com/r/'):
        body = util.get_content(submission.url)
    else:
        body = util.markdown(submission.selftext, output_format='html5')
    
    title = submission.title
    author = "[deleted]"
    if submission.author is not None:
        author = submission.author.name
    address = request.form['email']
    kindle_address = request.form['kindle_address']

    comments = None
    if request.form['comments'] == 'true':
        submission.comments.replace_more(limit=0)
        comments = util.get_comments(submission, request.form['comments_style'], author)

    attachment = render_template('comments.html', title=title, body=body, author=author,
                                 comments=comments)

    status = util.send_email(address, kindle_address, attachment, title)
    if status is None:
        return jsonify(type='success', text='Success!')
    else:
        return jsonify(type='warning', text='Uh oh! Something went wrong on our end')


@app.route('/subreddit', methods=['POST'])
def convert():
    if util.validate_request_subreddit(request.form) is not None:
        return jsonify(type='danger', text=util.validate_request_subreddit(request.form))

    subreddit = request.form['subreddit']
    include_comments = request.form['comments']
    time = request.form['time']
    limit = int(request.form['limit'])
    address = request.form['email']
    kindle_address = request.form['kindle_address']

    try:
        posts = util.get_posts(subreddit, time, limit)
        if time == 'all':
            title = 'Top ' + str(limit) + ' posts from /r/' + subreddit + ' ever'
        else:
            title = 'Top ' + str(limit) + ' posts from /r/' + subreddit + ' over the past ' + time
        top = []
        for post in posts:
            author = '[deleted]' if post.author is None else post.author.name
            comments = None
            if include_comments == 'true':
                post.comments.replace_more(limit=0)
                comments = util.get_comments(post, request.form['comments_style'], author)
            try:
                top.append({'title': post.title,
                            'body': util.get_content(post.url) if not post.url.startswith(
                                'https://www.reddit.com/r/') else util.markdown(
                                post.selftext),
                            'author': author,
                            'comments': comments})
            except:
                pass
    except:
        return jsonify(type='danger', text='That ain\'t no subreddit I\'ve ever heard of!')

    attachment = render_template('posts.html', posts=top, title=title)

    status = util.send_email(address, kindle_address, attachment, title)

    if status is None:
        return jsonify(type='success', text='Success!')
    else:
        return jsonify(type='warning', text='Uh oh! Something went wrong on our end')


if __name__ == '__main__':
    app.run(debug=True)
