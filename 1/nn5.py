import numpy as np

# sigmoid function
def nonlin(x,deriv=False):
    if(deriv==True):
        return x*(1-x)
    return 1/(1+np.exp(-x))
    
# input dataset
X = np.array([[0,0,1],[0,1,1],[1,0,1],[1,1,1]])
    
# output dataset            
Y = np.array([[0],[1],[1],[0]])

# seed random numbers to make calculation
# deterministic (just a good practice)
np.random.seed(1)

# initialize weights randomly with mean 0
syn0 = 2*np.random.random((3,4)) - 1
syn1 = 2*np.random.random((4,1)) - 1

for iter in range(60000):

    # forward propagation
    l0 = X
    l1 = nonlin(np.dot(l0,syn0))
    l2 = nonlin(np.dot(l1, syn1))
    
    # Back propagation of errors using the chain rule. 
    l2_error = Y - l2
    if(iter % 10000) == 0:   # Only print the error every 10000 steps, to save time and limit the amount of output. 
        print("Error L2: " + str(np.mean(np.abs(l2_error))))
        

    # how much did we miss?
   # l1_error = l2_delta.dot(syn1.T)

    # multiply how much we missed by the 
    # slope of the sigmoid at the values in l1
    l2_delta = l2_error*nonlin(l2, deriv=True)
    
    l1_error = l2_delta.dot(syn1.T)
    
    l1_delta = l1_error * nonlin(l1,deriv=True)
    if(iter % 10000) == 0:   # Only print the error every 10000 steps, to save time and limit the amount of output. 
        print("Error L1: " + str(np.mean(np.abs(l1_error))))

    # update weights
    syn1 += l1.T.dot(l2_delta)
    syn0 += l0.T.dot(l1_delta)

print ("Output After Training for l1:")
print (l1)
print ("Output After training for l2")
print(l2)

