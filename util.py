import os
import re
import smtplib
from configparser import ConfigParser
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText

import praw
import requests
from markdown import markdown


def to_html_numbers(comment, op):
    result = markdown(comment.body) + \
             ('<footer class="op">' if comment.author.name == op else '<footer>') + \
             comment.author.name + '</footer>'
    children = ['<li>' + to_html_numbers(reply, op) + '</li>' for reply in comment.replies if
                reply.author is not None]
    if children:
        result += '<ol>' + ''.join(children) + '</ol>'
    return result


def to_html_quotes(comment, op):
    result = markdown(comment.body) + \
             ('<footer class="op">' if comment.author.name == op else '<footer>') + \
             comment.author.name + '</footer>'
    children = ['<blockquote>' + to_html_quotes(reply, op) + '</blockquote>' for reply in
                comment.replies if
                reply.author is not None]
    if children:
        result += ''.join(children)
    return result


def get_comments(submission, comments_style, op):
    out = '<ol>' if comments_style == 'numbers' else ''
    for comment in submission.comments:
        if comment.author is not None:
            if comments_style == 'numbers':
                out += '<li>' + to_html_numbers(comment, op) + '</li>'
            else:
                out += '<blockquote>' + to_html_quotes(comment, op) + '</blockquote>'
    return out + ('</ol>' if comments_style == 'numbers' else '')


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


def get_mercury_token():
    if os.path.isfile(os.path.join(os.path.dirname(__file__), 'settings.cfg')):
        config = ConfigParser()
        config.read(os.path.join(os.path.dirname(__file__), 'settings.cfg'))
        token = config.get('mercury', 'token')
    else:
        token = os.environ['MERCURY_TOKEN']
    return token


def get_reddit_oauth():
    if os.path.isfile(os.path.join(os.path.dirname(__file__), 'settings.cfg')):
        config = ConfigParser()
        config.read(os.path.join(os.path.dirname(__file__), 'settings.cfg'))
        client_id = config.get('reddit', 'client_id')
        client_secret = config.get('reddit', 'client_secret')
    else:
        client_id = os.environ['CLIENT_ID']
        client_secret = os.environ['CLIENT_SECRET']
    return client_id, client_secret


def send_email(to, kindle_address, attachment, title):
    msg = MIMEMultipart()
    msg['From'] = 'convert@reddit2kindle.com'
    if kindle_address == 'free':
        msg['To'] = to + '@free.kindle.com'
    else:
        msg['To'] = to + '@kindle.com'
    title = "".join(c for c in title if c.isalnum() or c.isspace()).rstrip()
    msg['Subject'] = title

    attach = MIMEText(attachment.encode('iso-8859-1', 'xmlcharrefreplace'), 'html', 'iso-8859-1')
    attach.add_header('Content-Disposition', 'attachment',
                      filename=title + '.html')
    msg.attach(attach)

    s = smtplib.SMTP(get_smtp()[0], get_smtp()[1])

    s.login(get_auth()[0], get_auth()[1])
    s.send_message(msg)

    s.quit()


def validate_request_post(values):
    if values['submission'] is '':
        return 'You need to put a URL in!'
    if values['email'] is '':
        return 'How am I supposed to send it to you without an email address?'
    if values['kindle_address'] not in ['free', 'normal']:
        return 'Which kindle address do you want me to send to?'
    return None


def validate_request_subreddit(values):
    if values['subreddit'] is '':
        return 'I need a subreddit name!'
    if values['time'] not in ['all', 'year', 'month', 'week', 'day', 'hour']:
        return 'That\'s not a valid time period, is it?'
    try:
        if values['limit'] is '' or 0 > int(values['limit']) or int(values['limit']) > 25:
            return 'How many posts would you like?'
    except ValueError:
        return 'How many posts would you like?'
    if values['email'] is '':
        return 'How am I supposed to send it to you without an email address?'
    if values['kindle_address'] not in ['free', 'normal']:
        return 'Which kindle address do you want me to send to?'
    return None


def get_posts(subreddit, time, limit):
    return r.subreddit(subreddit).top(time_filter=time, limit=limit)


def get_content(url):
    request = requests.get(
        'https://mercury.postlight.com/parser?url=' + url,
        headers={'x-api-key': get_mercury_token()})
    p = re.compile(r'<img.*?>')
    try:
        return p.sub('', request.json()['content'])
    except:
        return ''


r = praw.Reddit(user_agent='reddit2kindle', client_id=get_reddit_oauth()[0],
                client_secret=get_reddit_oauth()[1])
