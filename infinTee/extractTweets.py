__author__ = 'ankit'

import twitterConfig
import yweather
import urllib2
import json
import logging
import operator
import requests

twitter_api = twitterConfig.TwitterLogin().login_to_twitter()
logging.basicConfig(filename='error.log', level=logging.DEBUG)

def get_woeid(location):

    client = yweather.Client()
    return client.fetch_woeid(location)


def get_sentiment_of_trending_topics(woeid_str, location):

    woeid = int(woeid_str.strip())
    trends = twitter_api.trends.place(_id=woeid)
    trends_set = set([trend['name'] for trend in trends[0]['trends']])
    trend_sentiment_dict = dict()
    for trend in trends_set:
        trend_sentiment_dict[trend] = get_sentiments(trend, location)
    return trend_sentiment_dict


def get_sentiments(trend, location):

    responses = ''
    try:
        responses = requests.get("https://4458bd55bddd91711c6321224c0cfa51:uiSNiHEf5v@cdeservice.mybluemix.net/api/v1/messages/search?q=bio_location:'"+location+"' "+trend+"&size=200")
    except:
        msg = "Error in get_list_sentiments() for: " + trend + location
        logging.error(msg=msg)
    data = responses.json()
    sentiment_list = []
    try:
        for idx, val in enumerate(data['tweets']):
            if 'content' in val['cde'].keys():
                sentiment_list.append(val['cde']['content']['sentiment']['polarity'])
    except:
        msg = "polarity not available for " + trend + location
        logging.error(msg=msg)
    return sentiment_list


def get_aggregate_sentiment(sentiment_list):

    sent = {'positive': 0, 'negative': 0, 'neutral': 0}

    if len(sentiment_list) == 0:
        return 'neutral'
    neutral = 1
    for sentiment in sentiment_list:

        if sentiment.encode('utf-8').lower() == 'positive':
            sent['positive'] += 1
        elif sentiment.encode('utf-8').lower() == 'negative':
            sent['negative'] += 1
        elif sentiment.encode('utf-8').lower() == 'ambivalent':
            sent['positive'] += 1
            sent['negative'] += 1
        else:
            if neutral == 3:
                sent['neutral'] += 1
                neutral = 1
            else:
                neutral += 1
    sorted_sent = sorted(sent.items(), key=operator.itemgetter(1))
    print sorted_sent
    return sorted_sent[-1][0]

if __name__ == '__main__':
    location = 'Hyderabad'
    woeid = get_woeid(location)
    trend_sentiment_dict = get_sentiment_of_trending_topics(woeid, location)
    trend_sentiment_aggregate_dict = dict()
    for key in trend_sentiment_dict.keys():
        trend_sentiment_aggregate_dict[key] = get_aggregate_sentiment(trend_sentiment_dict[key])
    for key in trend_sentiment_aggregate_dict.keys():
        print key, trend_sentiment_aggregate_dict[key]




