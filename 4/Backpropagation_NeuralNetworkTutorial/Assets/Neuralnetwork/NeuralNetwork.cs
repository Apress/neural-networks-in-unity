using System;

/// <summary>
/// Simple MLP Neural Network
/// </summary>
public class NeuralNetwork
{

    int[] layer; //layer information
    Layer[] layers; //layers in the network

    /// <summary>
    /// Constructor setting up layers
    /// </summary>
    /// <param name="layer">Layers of this network</param>
    public NeuralNetwork(int[] layer)
    {
        //deep copy layers
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
            this.layer[i] = layer[i];

        //creates neural layers
        layers = new Layer[layer.Length-1];

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layer[i], layer[i+1]);
        }
    }

    /// <summary>
    /// High level feedforward for this network
    /// </summary>
    /// <param name="inputs">Inputs to be feed forwared</param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        //feed forward
        layers[0].FeedForward(inputs);
        for (int i = 1; i < layers.Length; i++)
        {
            layers[i].FeedForward(layers[i-1].outputs);
        }

        return layers[layers.Length - 1].outputs; //return output of last layer
    }

    /// <summary>
    /// High level back porpagation
    /// Note: It is expexted the one feed forward was done before this back prop.
    /// </summary>
    /// <param name="expected">The expected output form the last feedforward</param>
    public void BackProp(float[] expected)
    {
        // run over all layers backwards
        for (int i = layers.Length-1; i >=0; i--)
        {
            if(i == layers.Length - 1)
            {
                layers[i].BackPropOutput(expected); //back prop output
            }
            else
            {
                layers[i].BackPropHidden(layers[i+1].gamma, layers[i+1].weights); //back prop hidden
            }
        }

        //Update weights
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].UpdateWeights();
        }
    }

    /// <summary>
    /// Each individual layer in the ML{
    /// </summary>
    public class Layer
    {
        int numberOfInputs; //# of neurons in the previous layer
        int numberOfOuputs; //# of neurons in the current layer


        public float[] outputs; //outputs of this layer
        public float[] inputs; //inputs in into this layer
        public float[,] weights; //weights of this layer
        public float[,] weightsDelta; //deltas of this layer
        public float[] gamma; //gamma of this layer
        public float[] error; //error of the output layer

        public static Random random = new Random(); //Static random class variable

        /// <summary>
        /// Constructor initilizes vaiour data structures
        /// </summary>
        /// <param name="numberOfInputs">Number of neurons in the previous layer</param>
        /// <param name="numberOfOuputs">Number of neurons in the current layer</param>
        public Layer(int numberOfInputs, int numberOfOuputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOuputs = numberOfOuputs;

            //initilize datastructures
            outputs = new float[numberOfOuputs];
            inputs = new float[numberOfInputs];
            weights = new float[numberOfOuputs, numberOfInputs];
            weightsDelta = new float[numberOfOuputs, numberOfInputs];
            gamma = new float[numberOfOuputs];
            error = new float[numberOfOuputs];

            InitilizeWeights(); //initilize weights
        }

        /// <summary>
        /// Initilize weights between -0.5 and 0.5
        /// </summary>
        public void InitilizeWeights()
        {
            for (int i = 0; i < numberOfOuputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f;
                }
            }
        }

        /// <summary>
        /// Feedforward this layer with a given input
        /// </summary>
        /// <param name="inputs">The output values of the previous layer</param>
        /// <returns></returns>
        public float[] FeedForward(float[] inputs)
        {
            this.inputs = inputs;// keep shallow copy which can be used for back propagation

            //feed forwards
            for (int i = 0; i < numberOfOuputs; i++)
            {
                outputs[i] = 0;
                for (int j = 0; j < numberOfInputs; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }

                outputs[i] = (float)Math.Tanh(outputs[i]);
            }

            return outputs;
        }

        /// <summary>
        /// TanH derivate 
        /// </summary>
        /// <param name="value">An already computed TanH value</param>
        /// <returns></returns>
        public float TanHDer(float value)
        {
            return 1 - (value * value);
        }

        /// <summary>
        /// Back propagation for the output layer
        /// </summary>
        /// <param name="expected">The expected output</param>
        public void BackPropOutput(float[] expected)
        {
            //Error dervative of the cost function
            for (int i = 0; i < numberOfOuputs; i++)
                error[i] = outputs[i] - expected[i];

            //Gamma calculation
            for (int i = 0; i < numberOfOuputs; i++)
                gamma[i] = error[i] * TanHDer(outputs[i]);

            //Caluclating detla weights
            for (int i = 0; i < numberOfOuputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        /// <summary>
        /// Back propagation for the hidden layers
        /// </summary>
        /// <param name="gammaForward">the gamma value of the forward layer</param>
        /// <param name="weightsFoward">the weights of the forward layer</param>
        public void BackPropHidden(float[] gammaForward, float[,] weightsFoward)
        {
            //Caluclate new gamma using gamma sums of the forward layer
            for (int i = 0; i < numberOfOuputs; i++)
            {
                gamma[i] = 0;

                for (int j = 0; j < gammaForward.Length; j++)
                {
                    gamma[i] += gammaForward[j] * weightsFoward[j, i];
                }

                gamma[i] *= TanHDer(outputs[i]);
            }

            //Caluclating detla weights
            for (int i = 0; i < numberOfOuputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }  

        /// <summary>
        /// Updating weights
        /// </summary>
        public void UpdateWeights()
        {
            for (int i = 0; i < numberOfOuputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] -= weightsDelta[i, j]*0.033f;
                }
            }
        }
    }
}
