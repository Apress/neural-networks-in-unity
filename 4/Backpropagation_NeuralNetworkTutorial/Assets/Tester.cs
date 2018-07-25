using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 0 0 0    => 0
        // 0 0 1    => 1
        // 0 1 0    => 1
        // 0 1 1    => 0
        // 1 0 0    => 1
        // 1 0 1    => 0
        // 1 1 0    => 0
        // 1 1 1    => 1

        NeuralNetwork net = new NeuralNetwork(new int[] { 3, 25, 25, 1 }); //intiilize network

        //Itterate 5000 times and train each possible output
        //5000*8 = 40000 traning operations
        for (int i = 0; i < 5000; i++)
        {
            net.FeedForward(new float[] { 0, 0, 0 });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { 0, 0, 1 });
            net.BackProp(new float[] { 1 });

            net.FeedForward(new float[] { 0, 1, 0 });
            net.BackProp(new float[] { 1 });

            net.FeedForward(new float[] { 0, 1, 1 });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { 1, 0, 0 });
            net.BackProp(new float[] { 1 });

            net.FeedForward(new float[] { 1, 0, 1 });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { 1, 1, 0 });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { 1, 1, 1 });
            net.BackProp(new float[] { 1 });
        }


        //output to see if the network has learnt
        //WHICH IT HAS!!!!!
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 0, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 0, 1 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 1, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 1, 1 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 0, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 0, 1 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 1, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 1, 1 })[0]);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
