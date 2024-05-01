using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ConsoleApp1.Program;

namespace ConsoleApp1
{
    internal class Program
    {
        static int a, b, c, d; 
        static double w1, w2, w3;
        static BinaryTree<int> tree = new BinaryTree<int>();
        static void Main(string[] args)
        {
            Random random = new Random();
            a = random.Next(20);
            b = random.Next(a + 1, a + 6);
            c = random.Next(b + 1, b + 20);
            d = random.Next(c + 1, c + 30);

            int count_num = 8;
            int count_G, all_G = 0, need_count = count_num;
            bool flag;
            string xs;

            Console.WriteLine("Введите 8 разных значений х через пробел");
            List<int> list = new List<int> {0};

            while (all_G < count_num)
            {
                count_G = 0;
                flag = false;

                if (all_G != 0)
                {
                    need_count = count_num - all_G + 2;
                    Console.WriteLine("Недостаточно точек для построения дерева." + "\n" +
                    $"Пожалуйста, введите еще {need_count} чисел");
                }
                
                while (flag == false)
                {
                    xs = Console.ReadLine();
                    list = xs.Split(' ').Select(y => Convert.ToInt32(y)).ToList();
                    if (list.Count == need_count)
                    {
                        flag = true;
                        Console.WriteLine("\n");
                    }
                    else
                    {
                        Console.WriteLine($"Пожалуйста, введите {need_count} чисел");
                    }
                }
                count_G = Nechet_to_chet(list);
                all_G += count_G;
            }


            Console.WriteLine("Дерево:");
            Console.WriteLine("------------------------------------------------------");
            tree.Print();
            Console.WriteLine("\\nХотите добавить узел?(да/нет)");
            string ans = Console.ReadLine();

            while (ans == "да")
            {
                count_G = 0;
                Console.WriteLine("Назовите узел:");

                while (count_G == 0)
                {
                    xs = Console.ReadLine();
                    list = xs.Split(' ').Select(y => Convert.ToInt32(y)).ToList();
                    count_G = Nechet_to_chet(list);
                    if (count_G == 0)
                    {
                        Console.WriteLine("Введите другой узел");
                    }
                }

                Console.WriteLine("\nДерево:");
                Console.WriteLine("------------------------------------------------------");
                tree.Print();

                Console.WriteLine("\\nХотите добавить узел?(да/нет)");
                ans = Console.ReadLine();
            }
        }

        /*добавление четкого числа в дерево*/
        static int Nechet_to_chet(List<int> list)
        {
            int count = 0, x, G;
            for (int i = 0; i < list.Count(); i++)
            {
                x = list[i];
                G = Formula_G(x);
                if (G != -1)
                {
                    tree.Add(Convert.ToInt32(G));
                    count++;
                }
            }
            return count;
        }

        /*нахождение четкого числа*/
        static int Formula_G(int x)
        {
            double F, g = 0;
            double sum_f = 0;
            int G = 0;
            for (int i = x; i < x + 5; i++)
            {
                F = Formula_F(i);
                g += i * F;
                sum_f += F;
            }
            if (sum_f == 0)
            {
                G = -1;
            }
            else
            {
                G = Convert.ToInt32(g / sum_f);
                Console.WriteLine($"x = {x};\t  G = {G}");
            }
            return G;
        }

        /*нахождение нечеткого числа*/
        static double Formula_F(int x)
        {
            double a_f = a/1.0, b_f = b/1.0, c_f = c/1.0, d_f = d/1.0;
            if (x >= a_f && x < b_f)
            {
                w1 = 1;
            }
            else
            {
                w1 = 0;
            }

            if (x >= b_f && x < c_f)
            {
                w2 = 1;
            }
            else
            {
                w2 = 0;
            }

            if (x >= c_f && x < d_f)
            {
                w3 = 1;
            }
            else
            {
                w3 = 0;
            }
            double F = Math.Round(w1 * ((x - a_f) / (b_f - a_f)) + w2 + w3 * ((d_f - x) / (d_f - c_f)));
            return F;
        }

        /*узел*/
        public class Node<T> where T : IComparable<T>
        {
            public T val;
            public int count;
            public Node<T> left;
            public Node<T> right;

            public Node(T value)
            {
                this.count = 1;
                this.val = value;
                this.right = null;
                this.left = null;
            }
            public Node(T value, Node<T> left, Node<T> right)
            {
                this.val = value;
                this.count = 1;
                this.right = right;
                this.left = left;
            }
        }

        public class BinaryTree<T> where T : IComparable<T>
        {
            public Node<T> root = null;

            public void Print()
            {
                Print(root, 4);
            }

            private void Print(Node<T> node, int padding) //вывод дерева
            {
                if (node != null)
                {
                    if (node.right != null)
                    {
                        Print(node.right, padding + 4);
                    }
                    if (padding > 0)
                    {
                        Console.Write(" ".PadLeft(padding));
                    }
                    if (node.right != null)
                    {
                        Console.Write("/\n");
                        Console.Write(" ".PadLeft(padding));
                    }
                    Console.Write(node.val.ToString() + "\n ");
                    if (node.left != null)
                    {
                        Console.Write(" ".PadLeft(padding) + "\\\n");
                        Print(node.left, padding + 4);
                    }
                }
            }

            /*добавление узла*/
            public void Add(T value)
            {
                root = Add(root, value);
            }
            private Node<T> Add(Node<T> node, T value)
            {
                if (node == null)
                {
                    Node<T> new_tree = new Node<T>(value);
                    return new_tree;
                }
                else
                {
                    if (node.val.CompareTo(value) == 0)
                    {
                        node.count += 1;
                    }
                    else
                    {
                        if (node.val.CompareTo(value) == -1)
                        {
                            node.right = Add(node.right, value);
                        }
                        else
                        {
                            node.left = Add(node.left, value);
                        }
                    }
                }
                return node;
            }



            /*Удаление узла*/
            public void Remove(T value)
            {
                root = Remove(root, value);
            }

            /*нахождение родителя удаляемого узла*/
            private Node<T> FindNext(Node<T> node)
            {
                if (node.right == null)
                {
                    return null;
                }

                node = node.right;
                while (node.left != null)
                {
                    node = node.left;
                }
                return node;
            }

            /*удаление узла*/
            private Node<T> Remove(Node<T> node, T value, bool delete_all = false)
            {
                if (node.val.CompareTo(value) == 0) // случай, когда значение равно корню (ноде)
                {
                    if (node.count > 1 && delete_all == false)
                    {
                        node.count--;
                        return node;
                    }

                    if (node.left == null && node.right == null)
                    {
                        return null;
                    }
                    else if (node.left == null)
                    {
                        return node.right;
                    }
                    else if (node.right == null)
                    {
                        return node.left;
                    }
                    else
                    {
                        Node<T> next_node = FindNext(node);
                        node.val = next_node.val;
                        node.count = next_node.count;
                        node.right = Remove(node.right, next_node.val);
                        return node;
                    }
                }

                if (node.val.CompareTo(value) == -1)
                {
                    node.right = Remove(node.right, value, delete_all);
                }
                else
                {
                    node.left = Remove(node.left, value, delete_all);
                }

                return node;
            }

        }
    }
}
