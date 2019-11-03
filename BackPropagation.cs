using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public interface INeuralNetwork
    {

        /// <summary>
        /// Compute output vector by input vector
        /// </summary>
        /// <param name="inputVector">Input vector (double[])</param>
        /// <returns>Output vector (double[])</returns>
        double[] ComputeOutput(double[] inputVector);

        Stream Save();

        /// <summary>
        /// Train network with given inputs and outputs
        /// </summary>
        /// <param name="inputs">Set of input vectors</param>
        /// <param name="outputs">Set if output vectors</param>
        void Train(IList<DataItem<double>> data);
    }

    public interface INeuron
    {

        /// <summary>
        /// Weights of the neuron
        /// </summary>
        double[] Weights { get; }

        /// <summary>
        /// Offset/bias of neuron (default is 0)
        /// </summary>
        double Bias { get; set; }

        /// <summary>
        /// Compute NET of the neuron by input vector
        /// </summary>
        /// <param name="inputVector">Input vector (must be the same dimension as was set in SetDimension)</param>
        /// <returns>NET of neuron</returns>
        double NET(double[] inputVector);

        /// <summary>
        /// Compute state of neuron
        /// </summary>
        /// <param name="inputVector">Input vector (must be the same dimension as was set in SetDimension)</param>
        /// <returns>State of neuron</returns>
        double Activate(double[] inputVector);

        /// <summary>
        /// Last calculated state in Activate
        /// </summary>
        double LastState { get; set; }

        /// <summary>
        /// Last calculated NET in NET
        /// </summary>
        double LastNET { get; set; }

        IList<INeuron> Childs { get; }

        IList<INeuron> Parents { get; }

        IFunction ActivationFunction { get; set; }

        double dEdz { get; set; }
    }

    public interface IFunction
    {
        double Compute(double x);
        double ComputeFirstDerivative(double x);
    }

    public interface ILayer
    {

        /// <summary>
        /// Compute output of the layer
        /// </summary>
        /// <param name="inputVector">Input vector</param>
        /// <returns>Output vector</returns>
        double[] Compute(double[] inputVector);

        /// <summary>
        /// Get last output of the layer
        /// </summary>
        double[] LastOutput { get; }

        /// <summary>
        /// Get neurons of the layer
        /// </summary>
        INeuron[] Neurons { get; }

        /// <summary>
        /// Get input dimension of neurons
        /// </summary>
        int InputDimension { get; }
    }

    public interface IMultilayerNeuralNetwork : INeuralNetwork
    {
        /// <summary>
        /// Get array of layers of network
        /// </summary>
        ILayer[] Layers { get; }
    }

    public interface ILearningStrategy<T>
    {
        /// <summary>
        /// Train neural network
        /// </summary>
        /// <param name="network">Neural network for training</param>
        /// <param name="inputs">Set of input vectors</param>
        /// <param name="outputs">Set of output vectors</param>
        void Train(T network, IList<DataItem<double>> data);
    }

    public class DataItem<T>
    {
        private T[] _input = null;
        private T[] _output = null;

        public DataItem()
        {
        }

        public DataItem(T[] input, T[] output)
        {
            _input = input;
            _output = output;
        }

        public T[] Input
        {
            get { return _input; }
            set { _input = value; }
        }

        public T[] Output
        {
            get { return _output; }
            set { _output = value; }
        }
    }

    public class LearningAlgorithmConfig
    {

        public double LearningRate { get; set; }

        /// <summary>
        /// Size of the butch. -1 means fullbutch size. 
        /// </summary>
        public int BatchSize { get; set; }

        public double RegularizationFactor { get; set; }

        public int MaxEpoches { get; set; }

        /// <summary>
        /// If cumulative error for all training examples is less then MinError, then algorithm stops 
        /// </summary>
        public double MinError { get; set; }

        /// <summary>
        /// If cumulative error change for all training examples is less then MinErrorChange, then algorithm stops 
        /// </summary>
        public double MinErrorChange { get; set; }

        /// <summary>
        /// Function to minimize
        /// </summary>
        public IMetrics<double> ErrorFunction { get; set; }

    }
    public interface IMetrics<T>
    {
        double Calculate(T[] v1, T[] v2);

        /// <summary>
        /// Calculate value of partial derivative by v2[v2Index]
        /// </summary>
        T CalculatePartialDerivaitveByV2Index(T[] v1, T[] v2, int v2Index);
    }


    internal class BackPropagation : ILearningStrategy<IMultilayerNeuralNetwork>
    {

        private LearningAlgorithmConfig _config = null;
        private Random _random = null;

        internal BackPropagation(LearningAlgorithmConfig config)
        {
            _config = config;
            _random = new Random(1);
        }





        public void Train(IMultilayerNeuralNetwork network, IList<DataItem<double>> data)
        {
            if (_config.BatchSize < 1 || _config.BatchSize > data.Count)
            {
                _config.BatchSize = data.Count;
            }
            double currentError = Single.MaxValue;
            double lastError = 0;
            int epochNumber = 0;
            Console.WriteLine("Start learning...");
            do
            {
                lastError = currentError;
                DateTime dtStart = DateTime.Now;

                #region one epoche

                //preparation for epoche
                int[] trainingIndices = new int[data.Count];
                for (int i = 0; i < data.Count; i++)
                {
                    trainingIndices[i] = i;
                }
                if (_config.BatchSize > 0)
                {
                    trainingIndices = Shuffle(trainingIndices);
                }





                //process data set
                int currentIndex = 0;
                do
                {


                    //accumulated error for batch, for weights and biases
                    double[][][] nablaWeights = new double[network.Layers.Length][][];
                    double[][] nablaBiases = new double[network.Layers.Length][];

                    //process one batch
                    for (int inBatchIndex = currentIndex; inBatchIndex < currentIndex + _config.BatchSize && inBatchIndex < data.Count; inBatchIndex++)
                    {
                        //forward pass
                        double[] realOutput = network.ComputeOutput(data[trainingIndices[inBatchIndex]].Input);

                        //backward pass, error propagation
                        //last layer
                        nablaWeights[network.Layers.Length - 1] = new double[network.Layers[network.Layers.Length - 1].Neurons.Length][];
                        nablaBiases[network.Layers.Length - 1] = new double[network.Layers[network.Layers.Length - 1].Neurons.Length];
                        for (int j = 0; j < network.Layers[network.Layers.Length - 1].Neurons.Length; j++)
                        {
                            network.Layers[network.Layers.Length - 1].Neurons[j].dEdz =
                                _config.ErrorFunction.CalculatePartialDerivaitveByV2Index(data[inBatchIndex].Output,
                                                                                          realOutput, j) *
                                network.Layers[network.Layers.Length - 1].Neurons[j].ActivationFunction.
                                    ComputeFirstDerivative(network.Layers[network.Layers.Length - 1].Neurons[j].LastNET);

                            nablaBiases[network.Layers.Length - 1][j] = _config.LearningRate *
                                                                        network.Layers[network.Layers.Length - 1].Neurons[j].dEdz;

                            nablaWeights[network.Layers.Length - 1][j] = new double[network.Layers[network.Layers.Length - 1].Neurons[j].Weights.Length];
                            for (int i = 0; i < network.Layers[network.Layers.Length - 1].Neurons[j].Weights.Length; i++)
                            {
                                nablaWeights[network.Layers.Length - 1][j][i] =
                                    _config.LearningRate * (network.Layers[network.Layers.Length - 1].Neurons[j].dEdz *
                                                          (network.Layers.Length > 1 ?
                                                                network.Layers[network.Layers.Length - 1 - 1].Neurons[i].LastState :
                                                                data[inBatchIndex].Input[i])
                                                               +
                                                          _config.RegularizationFactor *
                                                          network.Layers[network.Layers.Length - 1].Neurons[j].Weights[i]
                                                              / data.Count);
                            }
                        }

                        //hidden layers
                        for (int hiddenLayerIndex = network.Layers.Length - 2; hiddenLayerIndex >= 0; hiddenLayerIndex--)
                        {
                            nablaWeights[hiddenLayerIndex] = new double[network.Layers[hiddenLayerIndex].Neurons.Length][];
                            nablaBiases[hiddenLayerIndex] = new double[network.Layers[hiddenLayerIndex].Neurons.Length];
                            for (int j = 0; j < network.Layers[hiddenLayerIndex].Neurons.Length; j++)
                            {
                                network.Layers[hiddenLayerIndex].Neurons[j].dEdz = 0;
                                for (int k = 0; k < network.Layers[hiddenLayerIndex + 1].Neurons.Length; k++)
                                {
                                    network.Layers[hiddenLayerIndex].Neurons[j].dEdz +=
                                        network.Layers[hiddenLayerIndex + 1].Neurons[k].Weights[j] *
                                        network.Layers[hiddenLayerIndex + 1].Neurons[k].dEdz;
                                }
                                network.Layers[hiddenLayerIndex].Neurons[j].dEdz *=
                                    network.Layers[hiddenLayerIndex].Neurons[j].ActivationFunction.
                                        ComputeFirstDerivative(
                                            network.Layers[hiddenLayerIndex].Neurons[j].LastNET
                                        );

                                nablaBiases[hiddenLayerIndex][j] = _config.LearningRate *
                                                                   network.Layers[hiddenLayerIndex].Neurons[j].dEdz;

                                nablaWeights[hiddenLayerIndex][j] = new double[network.Layers[hiddenLayerIndex].Neurons[j].Weights.Length];
                                for (int i = 0; i < network.Layers[hiddenLayerIndex].Neurons[j].Weights.Length; i++)
                                {
                                    nablaWeights[hiddenLayerIndex][j][i] = _config.LearningRate * (
                                        network.Layers[hiddenLayerIndex].Neurons[j].dEdz *
                                        (hiddenLayerIndex > 0 ? network.Layers[hiddenLayerIndex - 1].Neurons[i].LastState : data[inBatchIndex].Input[i])
                                            +
                                        _config.RegularizationFactor * network.Layers[hiddenLayerIndex].Neurons[j].Weights[i] / data.Count
                                        );
                                }
                            }
                        }
                    }

                    //update weights and bias
                    for (int layerIndex = 0; layerIndex < network.Layers.Length; layerIndex++)
                    {
                        for (int neuronIndex = 0; neuronIndex < network.Layers[layerIndex].Neurons.Length; neuronIndex++)
                        {
                            network.Layers[layerIndex].Neurons[neuronIndex].Bias -= nablaBiases[layerIndex][neuronIndex];
                            for (int weightIndex = 0; weightIndex < network.Layers[layerIndex].Neurons[neuronIndex].Weights.Length; weightIndex++)
                            {
                                network.Layers[layerIndex].Neurons[neuronIndex].Weights[weightIndex] -=
                                    nablaWeights[layerIndex][neuronIndex][weightIndex];
                            }
                        }
                    }

                    currentIndex += _config.BatchSize;
                } while (currentIndex < data.Count);

                //recalculating error on all data
                currentError = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    double[] realOutput = network.ComputeOutput(data[i].Input);
                    currentError += _config.ErrorFunction.Calculate(data[i].Output, realOutput);
                }
                currentError *= 1d / data.Count;

                #endregion

                epochNumber++;
                Console.WriteLine( "Eposh #" + epochNumber.ToString() +
                                    " finished; current error is " + currentError.ToString() +
                                    "; it takes: " +
                                    (DateTime.Now - dtStart).Duration().ToString());
            } while (epochNumber < _config.MaxEpoches &&
                     currentError > _config.MinError &&
                     Math.Abs(currentError - lastError) > _config.MinErrorChange);
        }

        private int[] Shuffle(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (_random.NextDouble() >= 0.3d)
                {
                    int newIndex = _random.Next(arr.Length);
                    int tmp = arr[i];
                    arr[i] = arr[newIndex];
                    arr[newIndex] = tmp;
                }
            }
            return arr;
        }
    }
}
