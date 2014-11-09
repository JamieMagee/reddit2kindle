import os

from flask import Flask, request, jsonify
from flask.templating import render_template

import util
import forms


app = Flask(__name__)
app.secret_key = os.urandom(24)
forms.csrf.init_app(app)


@app.route('/')
def index():
    form = forms.Submission()
    return render_template('index.html', form=form)


@app.route('/thread', methods=['POST'])
def thread():
    if util.validate_request(request.form) is not None:
        return jsonify(type='danger', text=util.validate_request(request.form))

    try:
        submission = util.r.get_submission(url=request.form['submission'])
    except:
        return jsonify(type='danger', text='That wasn\'t a reddit link, was it?')
    submission.replace_more_comments(limit=0)

    comments = util.get_comments(submission)
    body = util.markdown(submission.selftext, output_format='html5')
    title = submission.title
    author = submission.author.name
    address = request.form['email']

    attachment = render_template('comments.html', title=title, body=body, author=author, comments=comments)

    status = util.send_email(address, attachment, title)

    if status is None:
        return jsonify(type='success', text='Success!')
    else:
        return jsonify(type='warning', text='Uh oh! Something went wrong on our end')


@app.route('/top', methods=['POST'])
def convert():
    subreddit = util.r.get_subreddit(request.form['subreddit'])
    time = request.form['time']
    limit = int(request.form['limit'])

    subreddit_data = util.get_posts(subreddit, time, limit)
    results = util.parse_subreddit_data(subreddit_data)

    return render_template('results.html', results=results, subreddit=subreddit, time=time, limit=limit)


if __name__ == '__main__':
    app.run(debug=True)
