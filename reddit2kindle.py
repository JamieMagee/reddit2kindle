from flask import Flask, request, redirect, url_for, flash
from flask.templating import render_template
import util
import os

app = Flask(__name__)
app.secret_key = os.urandom(24)

@app.route('/')
def index():
    return render_template('index.html')


@app.route('/thread', methods=['POST'])
def thread():
    if util.validate_request(request.form) is not None:
        flash(util.validate_request(request.form), 'danger')
        return redirect(url_for('index', _external=True))

    try:
        submission = util.r.get_submission(url=request.form['submission'])
    except:
        flash('That wasn\'t a reddit link, was it?', 'danger')
        return redirect(url_for('index', _external=True))
    submission.replace_more_comments(limit=0)

    comments = util.get_comments(submission)
    body = util.markdown(submission.selftext, output_format='html5')
    title = submission.title
    author = submission.author.name
    address = request.form['email']

    attachment = render_template('comments.html', title=title, body=body, author=author, comments=comments)

    status, msg = util.send_email(address, attachment, title)

    if status == 200:
        flash('Success!', 'success')
        return redirect(url_for('index', _external=True))
    else:
        flash('Uh oh! Something went wrong on our end', 'warning')
        return redirect(url_for('index', _external=True))


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
