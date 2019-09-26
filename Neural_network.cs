using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public enum Region
    {
        first = 1,
        second = 2,
        third = 3,
        fourth = 4
    }
    public class Coordinates
    {
        public double x { get; set; }
        public double y { get; set; }

        public Region Region { get; set; }
        public Coordinates(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Coordinates((double, double) coord)
        {
            x = coord.Item1;
            y = coord.Item2;
        }
        public void Set_class(Region region) =>
            Region = region;
    }
    class Neuron
    {
        public double width_x { get; set; }
        public double width_y { get; set; }

        public Neuron((double, double) weights)
        {
            width_x = weights.Item1;
            width_y = weights.Item2;
        }

        public Neuron(double width_x, double width_y)
        {
            this.width_x = width_x;
            this.width_y = width_y;
        }

        /// <summary>
        /// Сумматор 
        /// </summary>
        /// <param name="coord">Координаты точки</param>
        /// <returns>перемноженные координаты на весы</returns>
        public double get_value(Coordinates coord) =>
            coord.x * width_x + coord.y * width_y;

        /// <summary>
        /// Возвращает значение сигмоиды
        /// </summary>
        /// <param name="coord">Координаты точки для которой надо посчитать</param>
        /// <returns>Возвращает значение сигмоиды</returns>
        public double Sigmoida(Coordinates coord) =>
            1 / (1 + Math.Exp(-1 * get_value(coord)));

        /// <summary>
        /// Функция активатор
        /// </summary>
        /// <param name="coord">Координаты точки</param>
        /// <returns>1 если сигмоида больше 0.5 иначе 0</returns>
        public int IsActive(Coordinates coord) =>
            Sigmoida(coord) > 0.5 ? 1 : 0;
    }
    public class Neural_network
    {
        public static void Training()
        {
            Random rand = new Random();
            List<(double, double)> points = new List<(double, double)>
            {
                (-2, 3), (-1, 2), (0, 1), (1, 1), (3, 1),
                (2, 0), (3, 0), (2, -1), (3, -1), (3, -2),
                (2, -3), (1, -2), (-1, 0), (-2, 0), (-3, 0),
                (-2, 1), (-3, 1), (-3, 2), (-4, 1), (-4, 2)
            };

            List<int> results = new List<int>
            {
                1, 1, 1, 1, 1,
                2, 2, 2, 2, 2,
                3, 3, 3, 3, 3,
                4, 4, 4, 4, 4
            };

            List<Neuron> neurons = new List<Neuron>();
            List<Coordinates> coordinates = new List<Coordinates>();
            for (int i = 0; i < points.Count; i++)
            {
                Coordinates coord = new Coordinates(points[i]);
                switch (results[i])
                {
                    case 1:
                        coord.Set_class(Region.first);
                        break;
                    case 2:
                        coord.Set_class(Region.second);
                        break;
                    case 3:
                        coord.Set_class(Region.third);
                        break;
                    case 4:
                        coord.Set_class(Region.fourth);
                        break;
                }
                coordinates.Add(coord);
            }
            
            for(int i = 0; i < 4; i++)
            {
                neurons.Add(new Neuron((rand.NextDouble(), rand.NextDouble())));
            }

            for(int i = 0; i < 10000; i++)
            {
                foreach(var coord in coordinates)
                {
                    if(coord.Region == Region.first)
                    {
                        double f = neurons[0].get_value(coord);
                        double delt = 1 - neurons[0].Sigmoida(coord);
                        neurons[0].width_x = neurons[0].width_x + 0.1 * delt * coord.x;
                        neurons[0].width_y = neurons[0].width_y + 0.1 * delt * coord.y;
                    }
                    else if(coord.Region == Region.second)
                    {
                        double f = neurons[1].get_value(coord);
                        double delt = 1 - neurons[1].Sigmoida(coord);
                        neurons[1].width_x = neurons[1].width_x + 0.1 * delt * coord.x;
                        neurons[1].width_y = neurons[1].width_y + 0.1 * delt * coord.y;
                    }
                    else if(coord.Region == Region.third)
                    {
                        double f = neurons[2].get_value(coord);
                        double delt = 1 - neurons[2].Sigmoida(coord);
                        neurons[2].width_x = neurons[2].width_x + 0.1 * delt * coord.x;
                        neurons[2].width_y = neurons[2].width_y + 0.1 * delt * coord.y;
                    }
                    else if(coord.Region == Region.fourth)
                    {
                        double f = neurons[3].get_value(coord);
                        double delt = 1 - neurons[3].Sigmoida(coord);
                        neurons[3].width_x = neurons[3].width_x + 0.1 * delt * coord.x;
                        neurons[3].width_y = neurons[3].width_y + 0.1 * delt * coord.y;
                    }
                }
            }
            while (true)
            {
                //string s = Console.ReadLine();
                //x = Convert.ToDouble(s);
                //s = Console.ReadLine();
                //y = Convert.ToDouble(s);

                (double, double) xy;
                double.TryParse(Console.ReadLine().Replace('.', ','), out xy.Item1);
                double.TryParse(Console.ReadLine().Replace('.', ','), out xy.Item2);
                Coordinates coord = new Coordinates(xy);

                neurons.ForEach(neuron =>
                {
                    //r = neuron.get_value(coord);

                    Console.WriteLine(neuron.IsActive(coord));
                });
                Console.ReadKey();
            }
        }
    }
}
