#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <list>
#include "Point.h"


using namespace std;

int main() {
	cout << "Hello";
	return 0;
}
vector<Point> generatingList(string path)
{
	string line;
	int counter = 1;
	int n;
	ifstream in(path);


		if (counter == 1 && int.TryParse(line, out n) && (n >= 3 && n <= 1000))
		{
			list<Point> ms;
			for (int i = 0; i < n; i++)
			{
				line = sr.ReadLine();
				var nums = line.Split(' ');
				ms.Add(new Point(int.Parse(nums[0]), int.Parse(nums[1])));
			}
			return ms;
		}
		else
		{
			throw exception("Неверные входные параметры!");
		}
	};
}
class Point {
public:
	int x;
	int y;
	Point(int a, int b) {
		if (a <= 10000)
			x = a;
		else
			throw "Неверное значение X!";

		if (b <= 10000)
			y = b;
		else
			throw "Неверное значение Y!";
	}
	virtual void ToString() {
		cout << x << " " << y;
	}
};
class Stack {
public:
	vector<Point> data;
	const int capacity = 1000;
	int maximum;
	int size;
	Stack(int size)
	{
		if (size < 3)
			throw ("Количество деревьев должно быть больше 3!");
		else
		{
			if (size >= 3 && size <= capacity)
			{
				//data = new List<T>();
				maximum = size;
			}
			else
				throw ("Максимальный размер стека 1000!");
		}
	}
	Stack() {
		//data = new List<T>();
		maximum = 1000;
	}
	bool isEmpty() {
		return data.empty();
	}
	void Push(Point el)
	{
		if (maximum != data.size())
			data.push_back(el);
		else
			throw "Достигнуто максимально возможное количество элементов!";
	}
	Point Pop()
	{
		if (isEmpty())
			throw "Стек пуст!";
		else
		{
			auto element = data[data.size() - 1];
			data.pop_back();
			return element;
		}
	}
	Point Peek()
	{
		if (isEmpty())
			throw "Стек пуст!";
		else
			return data.back();
	}
	Point NextToTop()
	{
		if (isEmpty())
			throw "Стек пуст!";
		else
		{
			return data[data.size() - 2];
		}
	}

};
