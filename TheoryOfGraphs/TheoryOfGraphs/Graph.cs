using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheory
{
    //ребро
    public class Edge
    {
        public string start;
        public string end;
        public int val;

        public Edge(string start, string end, int val)
        {
            this.start = start;
            this.end = end;
            this.val = val;
        }
    }
    class Graph
    {
        //список смежности
        private Dictionary<string, Dictionary<string, int>> graph;
        //список ребер
        private List<Edge> edge;

        private bool oriented = true; //ориентированный или неориент

        private bool weighed = true; //взвешенный/невзвешенный

        //матрица смежности
        public int[,] matrix;
        //матрица предков
        private int[,] parent;

        //свойство ориентированности
        public bool Oriented
        {
            get
            {
                return oriented;
            }
            set
            {
                oriented = value;
            }
        }

        //свойство взвешенности
        public bool Weighed
        {
            get
            {
                return weighed;
            }
            set
            {
                weighed = value;
            }
        }

        //получить весь граф
        public Dictionary<string, Dictionary<string, int>> DictGraph
        {
            get
            {
                return graph;
            }
        }

        //получить матрицу
        public int[,] GetMatrix
        {
            get
            {
                return matrix;
            }
        }
        //получить матрицу
        public int[,] GetParent
        {
            get
            {
                return parent;
            }
        }

        //конструктор по умолчанию
        public Graph()
        {
            graph = new Dictionary<string, Dictionary<string, int>>();
        }

        //конструктор копирования
        public Graph(Graph graph)
        {
            oriented = graph.Oriented;
            weighed = graph.Weighed;
            this.graph = new Dictionary<string, Dictionary<string, int>>(); //указатель на граф
            foreach (var item in graph.DictGraph)
            {
                string nameNode = "";
                nameNode += item.Key; //заносим главную вершину в строке
                Dictionary<string, int> vertex = new Dictionary<string, int>();
                //цикл по вершинам, смежных с главной
                foreach (var items in item.Value)
                {
                    vertex.Add(items.Key, items.Value); //добавляем по одной вершине, смежной с главной
                }
                this.graph.Add(nameNode, vertex); //добавили всю строку
            }
        }



        //конструктор добавления из файла
        public Graph(string filePath)
        {
            graph = new Dictionary<string, Dictionary<string, int>>();
            using (StreamReader file = new StreamReader(@filePath))
            {
                string[] orAndWei = file.ReadLine().Split();

                if (orAndWei[0] == "1")
                {
                    oriented = true;
                }
                else
                {
                    oriented = false;
                }
                if (orAndWei[1] == "1")
                    weighed = true;
                else
                    weighed = false;

                string tempStr;
                string[] nodeStr; //каждый узел
                string[] nodesStr; // вся строка 
                while ((tempStr = file.ReadLine()) != null)
                {
                    nodesStr = tempStr.Split();
                    Dictionary<string, int> tempNextNodes = new Dictionary<string, int>();
                    for (int i = 1; i < nodesStr.Length; i++)
                    {
                        nodeStr = nodesStr[i].Split(':');
                        tempNextNodes.Add(nodeStr[0], Convert.ToInt32(nodeStr[1]));
                    }
                    graph.Add(nodesStr[0], tempNextNodes); //вершина+узлы построчно
                }
            }
        }

        //посчитаем степень выхода вершины(число выходящих ребер из вершины). Если она равна 0, то выводим эту изолированную вершину
        public int GetCountOutDegree(string node)
        {
            return graph[node].Count;
        }

        //подсчет степени входа вершины
        public int GetInDegreeNode(string node)
        {
            int count = 0;
            foreach (var keyValue in graph)
            {
                foreach (var keyValue2 in keyValue.Value)//проходимся по смежным вершинам
                {
                    if (keyValue2.Key == node)//если с этой вершиной есть смежные, то прибавляем счётчик
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        //вывод всех изолированных вершин орграфа
        public void GetIsolatedVertex(Graph graph)
        {
            oriented = graph.Oriented;
            if (oriented)
            {
                Console.WriteLine("Изолированные вершины:");
                //проходимся циклом по всем вершинам графа
                foreach (var item in graph.GetNodes())
                {
                    if (graph.GetCountOutDegree(item) == 0 && graph.GetInDegreeNode(item) == 0)
                    {
                        //vert.Add(item);
                        Console.Write(item + " ");
                    }
                }

            }

        }

        //создание списка ребер
        public void SetEdge()
        {
            edge = new List<Edge>();
            foreach (var items in graph)
            {
                foreach (var item in items.Value)
                {
                    Edge tmp = new Edge(items.Key, item.Key, item.Value);
                    edge.Add(tmp);
                }
            }
        }

        //создание матрицы смежности
        public void SetMatrix()
        {
            matrix = new int[graph.Count, graph.Count];
            foreach (var items in graph)
            {
                int i = Convert.ToInt32(items.Key);
                foreach (var item in items.Value)
                {
                    int j = Convert.ToInt32(item.Key);
                    matrix[i, j] = item.Value;
                }
            }
            for (int index = 0; index < matrix.GetLength(0); index++)
            {
                for (int jndex = 0; jndex < matrix.GetLength(1); jndex++)
                {
                    if (index == jndex || matrix[index, jndex] == 0) //если вершина сама с собой или такого пути не существует(т.е. равно 0)
                    {
                        matrix[index, jndex] = int.MaxValue; //ставим в матрице максимальное число
                    }
                }
            }
        }

        //создание матрицы смежности
        public void SetParent()
        {
            parent = new int[graph.Count, graph.Count];
            for (int index = 0; index < parent.GetLength(0); index++)
            {
                for (int jndex = 0; jndex < parent.GetLength(1); jndex++)
                {
                    if (matrix[index, jndex] == int.MaxValue)
                    {
                        parent[index, jndex] = -1;
                    }
                    else
                    {
                        parent[index, jndex] = index + 1;
                    }
                }
            }
        }


        //сначала обращаемся к списку рёбер, далее просто меняем вершины местами, то есть ключевая вершина и текущая  и добавляем в новый пустой граф перевёрнутый орграф
        public void GetOriObr(Graph graph)
        {

            //создаём пустой новый граф
            Graph new_graph = new Graph();
            //добавляем туда все вершины исходного графа
            foreach (var items in graph.DictGraph)
            {
                new_graph.AddVertex(items.Key);
            }

            if (oriented)
            {
                edge = new List<Edge>();
                foreach (var items in graph.DictGraph)
                {
                    foreach (var item in items.Value)
                    {
                        new_graph.AddEdge(item.Key, items.Key, item.Value); //добавляем перевёрнутые дуги
                    }
                }

            }

            new_graph.ShowLstVertex();
        }


        //Вывод всех заходящих соседних вершин орграфа
        public void GetInAllVertex(Graph graph, string nameNode)
        {
            bool vert = graph.Equals(nameNode);
            //если эта вершина сущeствует и граф ориентированный
            if (graph.Oriented)
            {
                if (graph.GetInDegreeNode(nameNode) != 0)
                {
                    foreach (var keyValue in graph.DictGraph)
                    {
                        foreach (var keyValue2 in keyValue.Value)//проходимся по смежным вершинам
                        {
                            if (keyValue2.Key == nameNode)//если с этой вершиной есть смежные, то выводим эти вершины
                            {
                                Console.Write(keyValue.Key + " ");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Нет заходящих соседних вершин.");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Граф неориентированный или такой вершины нет!");
            }
        }

        //добавление вершины
        public bool AddVertex(string nameNode)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            if (graph.ContainsKey(nameNode))//есть ли такая вершина в графе?
            {
                return false;
            }
            {
                graph.Add(nameNode, dict);
                return true;
            }
        }

        //удаление вершины
        public bool DeleteVertex(string nameNode)
        {
            bool deleted = graph.Remove(nameNode);
            foreach (var keyValue in graph)
            {
                graph[keyValue.Key].Remove(nameNode); //удаляем вершину по ключу
            }
            if (deleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //удаление ребра
        public bool DeleteEdge(string fromNode, string toNode)
        {
            bool deletedEdge = false;
            if (String.IsNullOrEmpty(fromNode) || String.IsNullOrEmpty(toNode))//евсли оно уже не удалено
            {
                return false;
            }
            if (oriented)
            {
                foreach (var keyValue in graph)
                {
                    if (keyValue.Key.Equals(fromNode) && keyValue.Value.ContainsKey(toNode))//если есть данные значения
                    {
                        deletedEdge = true;
                        graph[keyValue.Key].Remove(toNode);
                    }
                }
            }
            else
            {
                foreach (var keyValue in graph)
                {
                    if (keyValue.Key.Equals(fromNode) && keyValue.Value.ContainsKey(toNode) || keyValue.Key.Equals(toNode) && keyValue.Value.ContainsKey(fromNode))
                    {
                        deletedEdge = true;
                        graph[keyValue.Key].Remove(toNode);
                        graph[keyValue.Key].Remove(fromNode);
                    }
                }
            }
            return deletedEdge;
        }

        //обход в ширину
        public List<string> BFS_Comp(ref Dictionary<string, bool> usedNodes)
        {
            Queue<string> nodes = new Queue<string>();
            string tmp = "";

            foreach (var item in usedNodes)
            {
                if (item.Value == false) //если вершина не просмотрена
                {
                    tmp = item.Key; //присваиваем значение вершины
                    nodes.Enqueue(item.Key); //добавляем в очередь
                    break; //выходим из цикла
                }
            }
            usedNodes[tmp] = true; //помечаем текущую вершину просмотренной

            while (nodes.Count != 0)
            {
                string vrtx = nodes.Peek(); //извлекаем вершину из начала очереди, не удаляя её
                nodes.Dequeue();//удаляем вершину из начала очереди
                foreach (var ver in graph[vrtx]) //проходимся по вершинам, смежным с текущей
                {
                    if (usedNodes.Keys.Contains(ver.Key) && usedNodes[ver.Key] == false) //смежная с данной существует и ещё не просмотрена
                    {
                        usedNodes[ver.Key] = true; //помечаем смежную как просмотренную
                        nodes.Enqueue(ver.Key); //помещаем в очередь смежную вершину
                    }
                }
            }

            List<string> lst = new List<string>();
            foreach (var component in usedNodes)
            {
                if (component.Value == true)
                    lst.Add(component.Key); //добавляем в список просмотренные вершины
            }

            Dictionary<string, bool> tempDic = new Dictionary<string, bool>();
            foreach (var item in usedNodes)
            {
                if (item.Value == false) //если текущая вершина не просмотрена
                    tempDic.Add(item.Key, item.Value); //добавляем её во временной словарик (значение+булевская)
            }
            usedNodes.Clear(); //очистили словарь
            foreach (var item in tempDic)
                usedNodes.Add(item.Key, item.Value); //добавили все оставшиеся непросмотренные вершины
            return lst; //возвращаем компоненту

        }

        //обход в ширину + поиск кратчайших путей
        public void BFS(string nameNode, ref Dictionary<string, bool> usedNodes)
        {
            Queue<string> nodes = new Queue<string>();
            string tmp = "";

            Dictionary<string, int> dst = new Dictionary<string, int>();

            foreach (var v in graph.Keys)
            {
                dst.Add(v, 0);
            }

            nodes.Enqueue(nameNode);
            usedNodes[nameNode] = true; //помечаем текущую вершину просмотренной

            while (nodes.Count != 0)
            {
                string vrtx = nodes.Peek(); //извлекаем вершину из начала очереди, не удаляя её
                nodes.Dequeue(); //удаляем вершину из начала очереди
                foreach (var ver in graph[vrtx]) //проходимся по вершинам, смежным с текущей
                {
                    if (usedNodes.Keys.Contains(ver.Key) && usedNodes[ver.Key] == false) //смежная с данной существует и ещё не просмотрена
                    {
                        usedNodes[ver.Key] = true; //помечаем смежную как просмотренную

                        nodes.Enqueue(ver.Key); //помещаем в очередь смежную вершину

                        dst[ver.Key] = dst[vrtx] + 1; //считаем расстояние до вершины

                    }
                }
            }

            foreach (var ver in dst)
            {
                if (ver.Value != 0)
                {
                    Console.WriteLine("Длина пути между вершиной " + nameNode + " и вершиной " + ver.Key + " равна: " + ver.Value.ToString());
                }
                else
                {
                    Console.WriteLine("Нет пути между вершиной " + nameNode + " и вершиной " + ver.Key);
                }
            }
        }


        //добавление ребра
        public bool AddEdge(string fromNode, string toNode, int weight = 0)
        {
            if (oriented)
            {
                if (graph[fromNode].ContainsKey(toNode) == false && graph[fromNode].ContainsKey(toNode) == false) //если нет таких рёбер
                {
                    graph[fromNode].Add(toNode, weight);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (graph.ContainsKey(fromNode) && graph.ContainsKey(toNode))
                {
                    if (graph[fromNode].ContainsKey(toNode) == false && graph[fromNode].ContainsKey(toNode) == false)//если ещё граф не содержит введёные значения(т.е рёбра)
                    {
                        graph[fromNode].Add(toNode, weight);
                        graph[toNode].Add(fromNode, weight);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        //все вершины графа
        public string[] GetNodes()
        {
            string[] node = new string[graph.Count];
            int i = 0;
            foreach (var item in graph)
            {
                node[i] = item.Key;
                i++;
            }
            return node;
        }


        public Graph(List<string> v, List<Edge> edges)
        {

            graph = new Dictionary<string, Dictionary<string, int>>();
            oriented = false;
            weighed = true;
            //добавляем вершины в граф
            foreach (var item in v)
            {
                AddVertex(item);
            }
            foreach (var item in edges)
            {
                AddEdge(item.start, item.end, item.val);
            }

        }

        //алгоритм Прима
        public Graph Prima(List<string> notUseV)
        {

            //неиспользованные ребра
            SetEdge();
            //использованные вершины
            List<string> useV = new List<string>();
            //неиспользованные вершины
            List<Edge> MST = new List<Edge>();

            useV.Add(notUseV[0]); //в использованные верш добавляется текущая вершина(Саратов)
            notUseV.Remove(notUseV[0]); //и эта же вершина удаляется из неиспольз-ых
            string v; //предыдущая вершина
            string curV = notUseV[0]; //текущей вершиной становится следующая (Волгоград)
            while (notUseV.Count > 0)
            {
                int minE = -1;
                int min = int.MaxValue;
                //поиск наименьшего ребра
                for (int i = 0; i < edge.Count; i++)
                {
                    if (useV.IndexOf(edge[i].start) != -1 && useV.IndexOf(edge[i].end) == -1) //если сущ-ет такое начало и конец(Саратов-Москва)
                    {
                        if (min > edge[i].val) //ищем минимальное ребро между вершинами
                        {
                            min = edge[i].val;
                            curV = edge[i].end; //текущей вершиной становится конец ребра(Москва)
                            minE = i; //индекс миним вершины(конец у Саратова - это Волгоград)
                        }
                    }
                }

                v = curV; //предыдущей становится текущая
                useV.Add(v); //она добавляется в использ-ые вершины
                notUseV.Remove(notUseV[0]); //удаляем её из неиспольз-ых

                if (minE >= 0)//если индекс наим конца ребра >=0
                {

                    MST.Add(edge[minE]); //добавили в список это ребро. Мне нужно добавить в граф это ребро!

                }
            }

            Graph graph_ = new Graph(useV, MST);

            return graph_;
        }

        public int CountVrtx()
        {
            return graph.Count();
        }


        //алгоритм Дейкстры
        public int Dijkstra(int st)
        {
            List<int> minWeight = new List<int>(graph.Count);//созд список для смежных вершин с st
            for (int i = 0; i < graph.Count; i++)
            {
                minWeight.Add(0);
            }
            bool[] visited = new bool[graph.Count];
            for (int i = 0; i < graph.Count; i++)
            {
                if (graph.ContainsKey(Convert.ToString(st)) && graph[Convert.ToString(st)].ContainsKey(Convert.ToString(i))) //есть ли в графе такая вершина и если текущая вершина смежная с st
                {
                    minWeight[i] = graph[Convert.ToString(st)][Convert.ToString(i)]; //то присваиваем минимальный вес из этой смежн вершины
                }
                else
                {
                    minWeight[i] = int.MaxValue;
                }
                visited[i] = false;
            }
            minWeight[st] = 0; //обозначили вес вершины(с самой собой) = 0
            int index = 0;
            int u = 0;

            List<int> p = new List<int>();
            for (int k = 0; k < graph.Count; k++)
            {
                p.Add(-1);
            }
            p[st] = st;
            for (int i = 0; i < graph.Count; i++)
            {
                int min = int.MaxValue;
                for (int j = 0; j < graph.Count; j++)
                {
                    if (!visited[j] && minWeight[j] < min) //если вершина не просмотрена и минвес меньше минимума
                    {
                        min = minWeight[j];//присваиваем минимум значению минвеса
                        index = j; //запоминаем индекс этого минимума
                    }
                }
                u = index;
                visited[u] = true; //пометили просмотренной

                for (int j = 0; j < graph.Count; j++)
                {
                    if (graph.ContainsKey(Convert.ToString(u)) && graph[Convert.ToString(u)].ContainsKey(Convert.ToString(j)))
                    {
                        int value = graph[Convert.ToString(u)][Convert.ToString(j)];
                        if (graph.ContainsKey(Convert.ToString(u)) && graph[Convert.ToString(u)].ContainsKey(Convert.ToString(j)))
                        {
                            if (!visited[j] && value != int.MaxValue && minWeight[u] != int.MaxValue && (minWeight[u] + value < minWeight[j])) //если не промотрена и значение в списке не равно максимуму и складываем веса и смотрим какой вес меньше(куда идти дальше)(по формуле Дейкстра)
                            {
                                minWeight[j] = minWeight[u] + value;
                            }
                            p[j] = u;
                        }
                    }
                }
            }

            int maxWeight = 0; //тк в грфе нет отриц весов
            foreach (var ver in minWeight)
            {
                if (ver != int.MaxValue && ver > maxWeight)
                {
                    maxWeight = ver;
                }
            }
            Console.WriteLine("Эксцентриситет вершины " + st + " равен: " + maxWeight);
            Console.WriteLine();

            return maxWeight;//возвращать эксцентриситет текущей вершины
        }

        //Поиск центра графа (минимальный радиус из всех радиусов графа)
        //хочу в список радиусов запихнуть эксцентриситеты, полученные из Дейкстры
        public void FindCentreGraph(Graph graph, int st)
        {
            List<int> eks = new List<int>();//создаем список эксцентриситетов всех вершин

            int k = 0;
            //находим индекс этой вершины в графе
            foreach (var vrtx in graph.GetNodes())
            {
                if (vrtx == st.ToString())
                {
                    break;
                }
                k++;
            }
            int tmp;
            for(int i = k; i < graph.CountVrtx(); i++)
            {
                tmp = Dijkstra(i);
                eks.Add(tmp);
            }
            
            int radius = int.MaxValue;
            foreach(var val in eks)
            {
                if(val != 0 && val < radius)
                {
                    radius = val; //нашли радиус графа, ещё бы путь построить...
                }
            }
            Console.WriteLine("Радиус графа = {0}", radius);

            Console.Write("Центром графа является вершина(вершины): ");
            //Находим центр графа
            for(int i = 0; i < eks.Count(); i++)
            {
                if(eks[i] == radius)
                {
                    Console.WriteLine("{0} ", i);
                }
            }
        }


        //алгоритм Флойда
        public void Floyd()
        {
            for (int k = 0; k < graph.Count; k++)
            {
                for (int i = 0; i < graph.Count; i++)
                {
                    for (int j = 0; j < graph.Count; j++)
                    {
                        if (matrix[i, k] < int.MaxValue && matrix[k, j] < int.MaxValue)
                        {
                            matrix[i, j] = Math.Min(matrix[i, j], matrix[i, k] + matrix[k, j]);
                            parent[i, j] = parent[i, k];
                        }
                    }
                }
            }
        }


        //алгоритм Беллмана-Форда
        public void BelmanFord(string st)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            foreach (var item in graph)
            {
                d.Add(item.Key, int.MaxValue); //список вершин со значениями бесконечности
            }
            d[st] = 0; //помечаем 0 возле вершины u(длина пути)
            string x = "";
            for (int i = 0; i < graph.Count; i++)//по кол-ву вершин
            {
                x = "";
                for (int j = 0; j < edge.Count; j++)//по кол-ву ребер
                {
                    if (d[edge[j].start] < int.MaxValue) //идём именно от единицы(смотрим по началу ребра)
                    {
                        if (d[edge[j].end] > d[edge[j].start] + edge[j].val)
                        {
                            d[edge[j].end] = Math.Max(int.MinValue, d[edge[j].start] + edge[j].val);//вычисляются кратчайшие расстояния
                            //p[edge[j].end] = edge[j].start;
                            x = edge[j].end;
                        }
                    }
                }
            }
           
            if (x == "") //Если нет отриц циклов
            {
                foreach(var val in d)
                {
                    
                    if(val.Value != int.MaxValue && val.Value != 0)
                    {
                        Console.WriteLine("Длина кратчайшего пути из вершины " + st + " до вершины " + val.Key + " равна " + val.Value);
                    }
                    else
                    {
                        Console.WriteLine("Пути из вершины " + st +" в вершину " + val.Key +" не существует");
                    }
                }
            }
            else
            {
                Console.WriteLine("Есть отрицательный цикл");
            }
        }


        //вывод графа в консоль
        public void ShowLstVertex()
        {
            foreach (var keyValue in graph)
            {
                Console.Write(keyValue.Key + "->");
                foreach (var keyValue2 in keyValue.Value) //цикл по весам
                {
                    if (weighed) //если взвеш, то добавляем в выод ещё и вес
                    {
                        Console.Write(keyValue2.Key + ":" + keyValue2.Value + " ");
                    }
                    else
                    {
                        Console.Write(keyValue2.Key + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //вывод матрицы смежности
        public void ShowMatrix()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == int.MaxValue)
                    {
                        Console.Write("oo\t");
                    }
                    else
                    {
                        Console.Write(matrix[i, j] + "\t");
                    }
                }
                Console.WriteLine();
            }
        }


        //вывод в файл
        public void OutPutInFile(string str)
        {
            string oneNode;
            using (StreamWriter FileOut = new StreamWriter(str, false))
            {
                foreach (var keyValue in graph)
                {
                    oneNode = keyValue.Key + " ";
                    foreach (var keyValue2 in keyValue.Value)
                    {
                        oneNode += (keyValue2.Key + ":" + keyValue2.Value + " ");
                    }
                    FileOut.WriteLine(oneNode);
                }
            }
        }
    }
}