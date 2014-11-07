import praw
from markdown import markdown
import sendgrid
import os
from configparser import ConfigParser


def to_html(comment):
    result = markdown(comment.body) + '<footer>' + comment.author.name + '</footer>'
    children = ['<blockquote>' + to_html(reply) + '</blockquote>' for reply in comment.replies if
                reply.author is not None]
    if children:
        result += ''.join(children)
    return result


def get_comments(submission):
    out = ''
    for comment in submission.comments:
        if comment.author is not None:
            out += '<blockquote>' + to_html(comment) + '</blockquote>'
    return out


def get_auth():
    if os.path.isfile(os.path.join(os.path.dirname(__file__), 'settings.cfg')):
        config = ConfigParser()
        config.read(os.path.join(os.path.dirname(__file__), 'settings.cfg'))
        username = config.get('auth', 'username')
        password = config.get('auth', 'password')
    else:
        username = os.environ['SENDGRID_USERNAME']
        password = os.environ['SENDGRID_PASSWORD']
    return username, password


def send_email(to, attachment, title):
    message = sendgrid.Mail()
    message.add_to(to + '@kindle.com')
    message.set_subject(title)
    message.set_text(' ')
    message.set_from('convert@reddit2kindle.com')
    message.add_attachment_stream('{}.html'.format(title), attachment)
    return sg.send(message)


def validate_request(values):
    if values['submission'] is '':
        return 'You need to put a URL in!'
    if values['email'] is '':
        return 'How am I supposed to send it to you without an email address?'
    return None

r = praw.Reddit(user_agent='reddit2kindle')
sg = sendgrid.SendGridClient(get_auth()[0], get_auth()[1])


def get_posts(subreddit, time, limit):
    if time == 'hour':
        return subreddit.get_top_from_hour(limit=limit)
    elif time == 'day':
        return subreddit.get_top_from_day(limit=limit)
    elif time == 'week':
        return subreddit.get_top_from_week(limit=limit)
    elif time == 'month':
        return subreddit.get_top_from_month(limit=limit)
    elif time == 'year':
        return subreddit.get_top_from_year(limit=limit)
    elif time == 'all':
        return subreddit.get_top_from_all(limit=limit)