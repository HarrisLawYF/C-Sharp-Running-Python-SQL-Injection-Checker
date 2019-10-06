# Natural Language Processing

# Importing the libraries
import numpy as np
import pandas as pd
import re
import nltk
import pickle
import sys
from sklearn.utils import shuffle
from nltk.corpus import stopwords
from nltk.stem.porter import PorterStemmer
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.svm import SVC
from sklearn.preprocessing import StandardScaler

classifier_filename = "çlassifer_model"
cv_filename = "cv_model"
sc_filename = "sc_model"
def getReviewString(data):
    review = re.sub('[^a-zA-Z\*\;\%\&\|]', ' ', data)
    review = review.lower()
    review = review.split()
    ps = PorterStemmer()
    review = [ps.stem(word) for word in review if not word in set(stopwords.words('english'))]
    review = ' '.join(review)
    return review

def generateCorpus(corpus,data):
    corpus.append(getReviewString(data))
    return corpus

def train():
    # Importing the dataset
    dataset = pd.read_csv('hacking.tsv', delimiter = '\t', quoting = 3, encoding = "ISO-8859-1")
    dataset = shuffle(dataset)
    dataset.reset_index(inplace=True, drop=True)

    # Cleaning the texts
    nltk.download('stopwords')
        
    corpus = []
    for i in range(0, 300):
        generateCorpus(corpus,dataset['Comments'][i])
    
    # Creating the Bag of Words model
    cv = CountVectorizer()
    X = cv.fit_transform(corpus).toarray()
    pickle.dump(cv, open(cv_filename, 'wb'))
    y = dataset.iloc[:, 1].values
    
    from sklearn.model_selection import train_test_split
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 0.20, random_state = 0)
    
    # Feature Scaling
    sc = StandardScaler()
    X_train = sc.fit_transform(X_train)
    X_test = sc.transform(X_test)
    pickle.dump(sc, open(sc_filename, 'wb'))
    
    # Fitting Kernel SVM to the Training set
    classifier = SVC(kernel = 'rbf', random_state = 0)
    classifier.fit(X_train, y_train)
    pickle.dump(classifier, open(classifier_filename, 'wb'))
    
    # Predicting the Test set results
    # TODO we can't afford to have Type2 false negative error here
    y_ver = classifier.predict(X_test)
    
    # Making the Confusion Matrix
    from sklearn.metrics import confusion_matrix
    cm = confusion_matrix(y_test, y_ver)
    accuracy = (cm[0][0]+cm[1][1])/(cm[0][0]+cm[0][1]+cm[1][0]+cm[1][1])
    print("Accuracy: "+str(accuracy))
    # Choose precision if you want to be more confident of your true positives.
    precision = cm[1][1]/(cm[1][1]+cm[0][1])
    print("Precision: "+str(precision))
    # Choose Recall if the idea of false positives is far better than false negatives
    recall = cm[1][1]/(cm[1][1]+cm[1][0])
    print("Recall: "+str(recall))
    # F1 score reaches its best value at 1 and worst at 0
    f1_score = 2*precision*recall/(precision+recall)
    print("F1 Score: "+str(f1_score))

def predict(text):
    classifier = pickle.load(open(classifier_filename, 'rb'))
    cv = pickle.load(open(cv_filename, 'rb'))
    sc = pickle.load(open(sc_filename, 'rb'))
    X_pred = cv.transform([text]).toarray()
    X_pred = sc.transform(X_pred)
    y_pred = classifier.predict(X_pred)
    return y_pred
    
#train()
result = predict(' '.join(sys.argv[1:]))
sys.stdout.flush()
print("\n"+str(result))
