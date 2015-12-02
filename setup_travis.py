#!/usr/bin/python3

import os


def main():
    template_file = open("ReplyBot/app.DEFAULT.config", "r")
    config = open("ReplyBot/app.config", "w")

    template = template_file.read()
    template = template.replace("key=\"ConsumerKey\" value=\"\"", "key=\"ConsumerKey\" value=\""+os.environ['ConsumerKey']+"\"")
    template = template.replace("key=\"ConsumerSecret\" value=\"\"", "key=\"ConsumerSecret\" value=\""+os.environ['ConsumerSecret']+"\"")
    template = template.replace("key=\"AccessToken\" value=\"\"", "key=\"AccessToken\" value=\""+os.environ['AccessToken']+"\"")
    template = template.replace("key=\"AccessTokenSecret\" value=\"\"", "key=\"AccessTokenSecret\" value=\""+os.environ['AccessTokenSecret']+"\"")

    config.write(template)

if __name__ == '__main__':
    main()

