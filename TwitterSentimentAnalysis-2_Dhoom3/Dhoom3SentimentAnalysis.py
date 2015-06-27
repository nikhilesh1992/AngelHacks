from __future__ import division
import twitter
import nltk
from nltk.corpus import stopwords

#========================================

def get_words_in_tweets(tweets):
    all_words = []
    for (words, sentiment) in tweets:
      all_words.extend(words)
    return all_words

def get_word_features(wordlist):
    wordlist = nltk.FreqDist(wordlist)
    word_features = wordlist.keys()
    return word_features

def extract_features(document):
    document_words = set(document)
    features = {}
    for word in word_features:
        features['contains(%s)' % word] = (word in document_words)
    return features

#========================================================

customstopwords = ['band', 'they', 'them']

#Load positive tweets into a list
p = open(r'F:\MSR\PythonPackages\PositiveSentimentTweetsDhoom3.txt', 'r')
postxt = p.readlines()

#Load negative tweets into a list
n = open(r'F:\MSR\PythonPackages\NegativeSentimentTweetsDhoom3.txt', 'r')
negtxt = n.readlines()

neglist = []
poslist = []

#Create a list of 'negatives' with the exact length of our negative tweet list.
for i in range(0,len(negtxt)):
    neglist.append('negative')

#Likewise for positive.
for i in range(0,len(postxt)):
    poslist.append('positive')

#Creates a list of tuples, with sentiment tagged.
pos_tweets = zip(postxt, poslist)
neg_tweets = zip(negtxt, neglist)

#Combines all of the tagged tweets to one large list.
taggedtweets = pos_tweets + neg_tweets

#============================================================

tweets = []

#Create a list of words in the tweet, within a tuple.
for (word, sentiment) in taggedtweets:
    word_filter = [i.lower() for i in word.split()]
    tweets.append((word_filter, sentiment))


# Prepare list of features from training set
word_features = get_word_features(get_words_in_tweets(tweets))
# Prepare the training set
training_set = nltk.classify.util.apply_features(extract_features, tweets)
# Train the classifier
classifier = nltk.NaiveBayesClassifier.train(training_set)

p.close()
n.close()
#================================ Twitter tweet to classify ===================

api = twitter.Api(consumer_key='degpfAjnbEO4eyGb5VeUQ',consumer_secret='4LdHM1ePHDRALE1033IffrWyBMWz8HEVYC1D4iPzE', \
access_token_key='42810195-yUeFpX2bifTXoEixgW2bssUWeSbZtSo9sMfayFfn1', access_token_secret='91KLTTfxzvZ2p1t2wySZ4j0Prn79J7WOGhdqVL34fnUaS')

countPositive = 0
countNegative = 0
#api.GetSearch(term="AAP",geocode="37.781157,-122.398720,1mi",
#until="2013-12-24",lang="en", count="10", result_type="popular")

#f=open('dhoom3Test1.txt','w')

msgs = []

for day in range(20, 28):
    searchDate = "2013-12-"+str(day)
    msgs += api.GetSearch(term="#Dhoom3", until=searchDate, lang="en", count="100")
    msgs += api.GetSearch(term="Dhoom3", until=searchDate, lang="en", count="100")
    msgs += api.GetSearch(term="@Dhoom3TheMovie", until=searchDate, lang="en", count="100")
    msgs += api.GetSearch(term="Dhoom:3", until=searchDate, lang="en", count="100")


print "#msg", len(msgs)
for msg in msgs:
    sentiment = classifier.classify(extract_features(msg.text))
    #print msg.text, sentiment + '\n'
    #f.write(msg.text.encode('utf8')+'\t'+"[["+sentiment+"]]"+'\n')
    if sentiment == 'positive':
        countPositive+=1
    elif sentiment == 'negative':
        countNegative+=1

#f.close()
print countPositive/(countPositive+countNegative), countPositive, countNegative
