reddit2kindle
=============

A web app to convert a reddit post and comments into an e-book, and send it directly to your kindle device or app

Now you can read your favourite subreddits from the comfort of your e-reader. Here is a preview of the web app and the output it produces

![Web app](https://i.imgur.com/T3bagbw.png)

![Output](http://i.imgur.com/u5Vpkpq.png)

Installation
============
1. Install [Python 3](https://www.python.org/downloads) and [pip](http://pip.readthedocs.org/en/latest/installing.html)
2. Install the required packages `pip install -r requirements.txt`
3. Either create a `settings.cfg` file of the format:

        [auth]
        username=johndoe@reddit2kindle.com
        password=hunter2
        [smtp]
        server=smtp.gmail.com
        port=587
or create environment variables for `SMTP_USERNAME`,`SMTP_PASSWORD`, `SMTP_SERVER`, and `SMTP_PORT`.
4. Run the app `python reddit2kindle.py`

Usage
=====
You need to add reddit2kindle to your approved Kindle e-mail senders. To do this:

1. Visit [Manage Your Content and Devices](http://www.amazon.co.uk/manageyourkindle) page.
2. Go to "Your Account", select "Manage Your Content and Devices" and then select "Personal Document Settings".
3. Under "Approved Personal Document E-mail List" click on "Add a new approved e-mail address".
4. Add "convert@reddit2kindle.com" and select "Add Address".

TODO
====
* Fix UTF-8 weirdness with the Kindle converter
* Add "Send top x posts from subreddit" feature
* Use [WTForms](https://flask-wtf.readthedocs.org/en/latest/) to add CSRF protection
