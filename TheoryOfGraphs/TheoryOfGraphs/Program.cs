using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheory
{
    class Program
    {
        static void Main(string[] args)
        {
        start:
            int val = 1;
            Console.Write("Нажмите 1 - создать пустой граф; 2 - считать с файла: ");
            Int32.TryParse(Console.ReadLine(), out val);
            Graph graph = new Graph();
            if (val == 1)
            {
                graph = new Graph();
                Console.Write("Граф будет ориентированным?(Да/Нет)");
                string str = Console.ReadLine();
                if (str == "Да")
                {
                    graph.Oriented = true;
                }
                else if (str == "Нет")
                {
                    graph.Oriented = false;
                }
                else
                {
                    Console.WriteLine("Неверно введены данные. Попробуйте ещё раз.");
                    goto start;
                }
                Console.Write("Граф будет взвешенным? ");
                str = Console.ReadLine();
                if (str == "Да")
                {
                    graph.Weighed = true;
                }
                else if (str == "Нет")
                {
                    graph.Weighed = false;
                }
                else
                {
                    Console.WriteLine("Неверно введены данные. Попробуйте ещё раз.");
                    goto start;
                }
            }
            else if (val == 2)
            {
                Console.WriteLine("Выберите файл: ");
                Console.WriteLine("1. Ориентированный взвешенный ");
                Console.WriteLine("2. Ориентированный невзвешенный ");
                Console.WriteLine("3. Неориентированный взвешенный ");
                Console.WriteLine("4. Неориетированный невзвешенный ");
                Console.WriteLine("5. Для компоненты связности: ");
                Console.Write("Введите число: ");
                string filePath = Console.ReadLine();
                if (filePath == "1")
                    graph = new Graph(@"C:\Users\Alina\source\repos\TheoryOfGraphs\TheoryOfGraphs\OriWeiGraph.txt");
                else if (filePath == "2")
                    graph = new Graph(@"C:\Users\Alina\source\repos\TheoryOfGraphs\TheoryOfGraphs\IsolatedVertexOri.txt");
                else if (filePath == "3")
                    graph = new Graph(@"C:\Users\Alina\source\repos\TheoryOfGraphs\TheoryOfGraphs\NoOriWei.txt");
                else if (filePath == "4")
                    graph = new Graph(@"C:\Users\Alina\source\repos\TheoryOfGraphs\TheoryOfGraphs\NoOriNoWei.txt");
                else if (filePath == "5")
                    graph = new Graph(@"C:\Users\Alina\source\repos\TheoryOfGraphs\TheoryOfGraphs\ComponentOri.txt");
                else
                {
                    Console.WriteLine("Нет такого числа");
                    return;
                }
            }
            while (val != 0)
            {
                Console.WriteLine("Меню для работы с графом:");
                Console.WriteLine("1.Добавить вершину");
                Console.WriteLine("2.Удалить вершину");
                Console.WriteLine("3.Добавить ребро");
                Console.WriteLine("4.Удалить ребро");
                Console.WriteLine("5.Просмотреть граф в консоли");
                Console.WriteLine("6.Загрузить в файл");
                Console.WriteLine("7.Показать изолированные вершины орграфа");
                Console.WriteLine("8.Показать все заходящие соседние вершины для заданной вершины");
                Console.WriteLine("9.Построить орграф, являющийся обращением данного орграфа(каждая дуга перевёрнута)");
                Console.WriteLine("10.Подсчитать количество связных компонент графа - ОБХОД В ГЛУБИНУ");
                Console.WriteLine("11.Найти длины кратчайших (по числу дуг) путей из данной вершины во все остальные - ОБХОД В ШИРИНУ");
                Console.WriteLine("12.Найти каркас минимального веса с помощью алгоритма Прима");
                Console.WriteLine("13.Вывести длины кратчайших путей для всех пар вершин - Флойд(№9)");
                Console.WriteLine("14.Вывести длины кратчайших путей от u до всех остальных вершин - Беллман-Форд(№5)");
                Console.WriteLine("15.Найти центр графа — множество вершин, эксцентриситеты которых равны радиусу графа - Дейкстра(№11)");
                Console.WriteLine("0.Выйти из программы");
                Console.Write("Введите число:");
                Int32.TryParse(Console.ReadLine(), out val);
                string str;
                string[] strArr;
                switch (val)
                {
                    case 1:
                        Console.WriteLine();
                        Console.Write("Введите название вершнины, которую нужно добавить: ");
                        str = Console.ReadLine();
                        if (graph.AddVertex(str))
                        {
                            Console.WriteLine("Вершина добавлена");
                        }
                        else
                        {
                            Console.WriteLine("Такая вершина уже есть. Добавлять можно только новые.");
                        }
                        Console.WriteLine();

                        break;
                    case 2:
                        Console.WriteLine();
                        Console.Write("Введите название вершины, которую нужно удалить: ");
                        str = Console.ReadLine();
                        if (graph.DeleteVertex(str))
                        {
                            Console.WriteLine("Вершина удалена.");
                        }
                        else
                        {
                            Console.WriteLine("Вершина не удалена. Проверьте данные.");
                        }
                        Console.WriteLine();
                        break;
                    case 3:
                        Console.WriteLine();
                        if (graph.Weighed)
                        {
                            Console.Write("Введите ('из', 'куда', 'вес') для добавления ребра: ");
                            strArr = Console.ReadLine().Split();
                            string[] vertex = graph.GetNodes(); //для проверки наличия вершин графа
                            if (strArr.Length == 3 && int.TryParse(strArr[2], out _) && vertex.Contains(strArr[0]) && vertex.Contains(strArr[1]))
                            {
                                if (graph.AddEdge(strArr[0], strArr[1], Convert.ToInt32(strArr[2])))
                                {
                                    Console.WriteLine("Ребро добавлено.");
                                }
                                else
                                {
                                    Console.WriteLine("Нет такой вершины или уже есть такое ребро. Добавление ребра не выполнено.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Введены неверно данные.");
                            }
                        }
                        else
                        {
                            Console.Write("Введите ('из', 'куда') для добавления ребра: ");
                            strArr = Console.ReadLine().Split();
                            string[] vertex = graph.GetNodes(); //проверяем наличие вершины в графе
                            if (strArr.Length == 2 && vertex.Contains(strArr[0]) && vertex.Contains(strArr[1]))
                            {
                                if (graph.AddEdge(strArr[0], strArr[1]))
                                {
                                    Console.WriteLine("Ребро добавлено.");
                                }
                                else
                                {
                                    Console.WriteLine("Нет такой вершины или уже есть такое ребро. Добавление ребра не выполнено.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Введены неверно данные.");
                            }
                        }
                        Console.WriteLine();
                        break;
                    case 4:
                        Console.WriteLine();
                        Console.Write("Введите ('из', 'куда') для удаления ребра: ");
                        strArr = Console.ReadLine().Split();
                        if (strArr.Length == 2)
                        {
                            if (graph.DeleteEdge(strArr[0], strArr[1]))
                            {
                                Console.WriteLine("Ребро удалено");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ребро не удалено, проверьте данные.");
                        }
                        Console.WriteLine();
                        break;
                    case 5:
                        Console.WriteLine();
                        graph.ShowLstVertex();
                        Console.WriteLine();
                        break;
                    case 6:
                        Console.WriteLine();
                        Console.WriteLine("Введите ссылку на файл.");
                        Graph graph1 = new Graph(graph);
                        str = Console.ReadLine();
                        graph1.OutPutInFile(str);
                        Console.WriteLine();
                        break;
                    case 7:
                        graph.GetIsolatedVertex(graph);
                        Console.WriteLine();
                        break;
                    case 8:
                        Console.Write("Введите название вершины: ");
                        str = Console.ReadLine();
                        graph.GetInAllVertex(graph, str);
                        Console.WriteLine();
                        break;
                    case 9:
                        Console.WriteLine();
                        graph.GetOriObr(graph);
                        Console.WriteLine();
                        break;
                    case 10:
                        Console.WriteLine();
                        Console.WriteLine("Происходит поиск компонент связности...");
                        string[] nodes = graph.GetNodes();
                        Dictionary<string, bool> usedNodes = new Dictionary<string, bool>();
                        List<List<string>> component = new List<List<string>>();
                        int count = 0;
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            usedNodes.Add(nodes[i], false);
                        }
                        while (usedNodes.Count != 0)
                        {
                            count++;
                            component.Add(graph.BFS_Comp(ref usedNodes));
                        }
                        for (int i = 0; i < component.Count; i++)
                        {
                            Console.WriteLine("Компонента");
                            for (int j = 0; j < component[i].Count; j++)
                            {
                                Console.Write(component[i][j] + " ");
                            }
                            Console.WriteLine();

                        }
                        Console.WriteLine("Найдено {0} связных компонент графа.", count);
                        Console.WriteLine();
                        break;
                    case 11:
                        nodes = graph.GetNodes(); //получаем все вершины графа
                        usedNodes = new Dictionary<string, bool>();
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            usedNodes.Add(nodes[i], false); //добавляем вершины в словарик - непросмотренные(false)
                        }
                        Console.Write("Введите название вершины: ");
                        string nameNode = Console.ReadLine();
                        graph.BFS(nameNode, ref usedNodes);
                        break;
                    case 12:
                        Console.WriteLine("Алгоритм Прима(поиск минимального оставного дерева).");
                        nodes = graph.GetNodes(); //получаем все вершины графа
                        usedNodes = new Dictionary<string, bool>();
                        component = new List<List<string>>();
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            usedNodes.Add(nodes[i], false); //добавляем вершины в словарик - непросмотренные(false)
                        }
                        //обход графа в ширину
                        while (usedNodes.Count != 0)
                        {
                            component.Add(graph.BFS_Comp(ref usedNodes)); //нашли кол-во компонент
                        }

                        Graph MST;
                        //сумма весов ребер(результат)
                        for (int i = 0; i < component.Count; i++)
                        {
                            Console.WriteLine("Минимальное оставное дерево:");
                            MST = graph.Prima(component[i]); //список минимальных рёбер

                            Console.WriteLine();
                            MST.ShowLstVertex();
                        }
                        Console.WriteLine();
                        break;
                    case 13:
                        Console.WriteLine();
                        graph.SetMatrix();
                        graph.ShowMatrix();
                        graph.SetParent();
                        int[,] prev = graph.GetParent;
                        for (int i = 0; i < prev.GetLength(1); i++)
                        {
                            for (int j = 0; j < prev.GetLength(1); j++)
                            {
                                Console.Write(prev[i, j] + " ");
                            }
                            Console.WriteLine();
                        }
                        graph.Floyd();
                        int[,] matrix = graph.GetMatrix;

                        for (int i = 0; i < matrix.GetLength(1); i++)
                        {
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                Console.Write(matrix[i, j] + " ");
                            }
                            Console.WriteLine();
                        } 

                        for (int i = 0; i < matrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                if (matrix[i, j] != int.MaxValue)
                                {
                                    Console.WriteLine("От вершины "+ i +" до вершины " + j + " длина кратчайшего пути равна " + matrix[i, j]);
                                }
                                else
                                {
                                    Console.WriteLine("От вершины " + i + " до вершины " + j + " пути не существует");
                                }
                            }
                            Console.WriteLine();   
                        }
                        break;
                    case 14:
                        Console.WriteLine();
                        Console.WriteLine("Введите вершину u: ");
                        string u1 = Console.ReadLine();
                        graph.SetEdge();
                        Console.WriteLine();
                        graph.BelmanFord(u1);
                        Console.WriteLine();
                        break;

                    case 15:
                        Console.WriteLine();
                        List<List<int>> minweight = new List<List<int>>();
                        int k = 0;
                        graph.FindCentreGraph(graph, k);
                        break;
                    case 0:
                        Console.WriteLine();
                        Console.WriteLine("Выход из программы.");
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Вы ввели не то число.");
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}