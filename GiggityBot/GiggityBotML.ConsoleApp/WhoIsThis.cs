using System;
using System.Collections.Generic;
using System.Text;
using GiggityBotML.Model;

namespace GiggityBotML.ConsoleApp
{
    public class WhoIsThis
    {
        public static string CompareImage(string pathToComparison)
        {
            try
            {
                ModelInput sampleData = new ModelInput()
                {
                    ImageSource = pathToComparison,
                };

                var predictionResult = ConsumeModel.Predict(sampleData);

                string _result = "Prediction: " + predictionResult.Prediction + " | Scores: " + predictionResult.Score;
                return _result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}