import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
import os
from configparser import ConfigParser

import praw
from markdown import markdown


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
        username = os.environ['SMTP_USERNAME']
        password = os.environ['SMTP_PASSWORD']
    return username, password


def get_smtp():
    if os.path.isfile(os.path.join(os.path.dirname(__file__), 'settings.cfg')):
        config = ConfigParser()
        config.read(os.path.join(os.path.dirname(__file__), 'settings.cfg'))
        server = config.get('smtp', 'server')
        port = config.get('smtp', 'port')
    else:
        server = os.environ['SMTP_SERVER']
        port = os.environ['SMTP_PORT']
    return server, port


def send_email(to, attachment, title):
    msg = MIMEMultipart()
    msg['From'] = 'convert@reddit2kindle.com'
    msg['To'] = to + '@kindle.com'
    msg['Subject'] = title

    attach = MIMEText(attachment, 'html', 'utf8')
    attach.add_header('Content-Disposition', 'attachment', filename=title + '.html')
    msg.attach(attach)

    s = smtplib.SMTP(get_smtp()[0], get_smtp()[1])

    s.login(get_auth()[0], get_auth()[1])
    s.send_message(msg)

    s.quit()


def validate_request(values):
    if values['submission'] is '':
        return 'You need to put a URL in!'
    if values['email'] is '':
        return 'How am I supposed to send it to you without an email address?'
    return None


r = praw.Reddit(user_agent='reddit2kindle')


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