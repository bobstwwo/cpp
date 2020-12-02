#include <iostream>
#include <math.h>
#include <vector>
#include <string>
#include <fstream>
#include <map>
#include <cmath>
#include <sstream>
#include <algorithm>
#include <iterator> 
using namespace std;

class Node {
	vector<int> keys;
	int* vals;
	int t;
	Node** C;
	int cntKeys;
	bool leaf;

public:

	Node(bool isLeaf, int t) {
		keys = vector<int>(2 * t - 1);
		vals = new int[2 * t - 1];
		leaf = isLeaf;
		this->t = t;
		C = new Node * [2 * t];
		cntKeys = 0;
	}
	friend class Tree;
	void decrem(int index, int k)
	{
		while (index >= 0 && keys[index] > k)
		{
			index--;
		}
	}
	void insertNonFull(int k, int val) {
		int index = cntKeys - 1;

		if (!leaf) {
			decrem(index, k);
			if (C[index + 1]->cntKeys == 2 * t - 1) {
				splitChild(index + 1, C[index + 1]);
				if (keys[index + 1] < k)
				{
					index++;
				}
			}
			C[index + 1]->insertNonFull(k, val);
		}
		else {
			while (index >= 0 && keys[index] > k) {
				keys[index + 1] = keys[index];
				vals[index + 1] = vals[index];
				index--;
			}
			keys[index + 1] = k;
			vals[index + 1] = val;
			cntKeys = cntKeys + 1;
		}
	}
	void inter(int t_amount,Node* node,Node* y)
	{
		for (int i = 0; i < t_amount; i++)
		{
			node->keys[i] = y->keys[i + t];
		}
	}
	void changeInter(int k)
	{
		for (int j = cntKeys - 1; k <= j; j--)
		{
			int j_one = j + 1;
			keys[j_one] = keys[j];
		}
	}
	void changeInt(int k)
	{
		for (int j = cntKeys; (k + 1) <= j; j--)
		{
			int j_one = j + 1;
			C[j_one] = C[j];
		}
	}
	void splitChild(int k, Node* y) {
		int t_amount = t - 1;
		Node* node = new Node(y->leaf, y->t);
		node->cntKeys = t_amount;
		inter(t_amount,node,y);
		if (y->leaf != true) {
			for (int i = 0; i < t_amount + 1; i++)
			{
				node->C[i] = y->C[i + t];
			}
		}
		y->cntKeys = t_amount;
		changeInt(k);
		C[k + 1] = node;
		changeInter(k);
		keys[k] = y->keys[t_amount];
		cntKeys = cntKeys + 1;
	}
	string find(int k) {
		int i = 0;
		while (i < cntKeys && keys[i] < k)
		{
			i = i + 1;
		}
		if (keys[i] != k)
		{
			return " ";
		}
		else
		{
			return to_string(this->vals[i]);
		}
		if (!leaf)
		{
			return C[i]->find(k);
		}
		else
		{
			return " ";
		}
	}
};

class Tree {
	Node* head = NULL;
	int t;

public:

	Tree(int t) {
		this->t = t;
	}

	string find(int k) {
		if (head != 0)
		{
			return head->find(k);
		}
		else
		{
			return " ";
		}
	}
	template<typename C, typename T>
	bool contains(C&& c, T e) {
		return std::find(begin(c), end(c), e) != end(c);
	};
	bool insert(int k, int val) {
		if ((head != NULL) && (contains(head->keys, k) == 1))
		{
			return false;
		}
		else
		{
			if (head != 0) {

				if (head->cntKeys != 2 * t - 1)
				{
					head->insertNonFull(k, val);
				}
				else
				{
					Node* s = new Node(false, t);
					s->C[0] = head;
					s->splitChild(0, head);
					int i = 0;
					if (s->keys[0] < k)
					{
						i++;
					}
					s->C[i]->insertNonFull(k, val);
					head = s;
				}
			}
			else {
				head = new Node(true, t);
				head->keys[0] = k;
				head->vals[0] = val;
				head->cntKeys = 1;
			}
			return true;
		}
	}
};


static vector<string> get_input(string path)
{
	ifstream fs;
	fs.open(path);
	vector<string> input_lines;
	if (fs.is_open())
	{
		int k = 0;
		while (!fs.eof())
		{
			string line = "";
			getline(fs, line);
			if (line == "")
			{
				continue;
			}
			input_lines.push_back(line);
		}
	}
	else
	{
		throw exception("File not found!");
	}
	return input_lines;
}
static void write_result(string path, string str)
{
	ofstream ofs;
	ofs.open(path);
	if (!ofs.is_open())
	{
		throw exception("Unable to open.");
	}
	else
	{
		ofs << str;
	}
}

int main(int argc, char* argv[]) {
	try
	{
		if (argc == 4) {
			int t = stoi(argv[1]);
			string path_to_input = argv[2];
			string path_to_output = argv[3];
			string result = "";
			vector<string> data = get_input(path_to_input);
			Tree tree(t);
			for (size_t i = 0; i < data.size(); i++)
			{
				istringstream iss(data[i]);
				vector<string> v((istream_iterator<string>(iss)),
					istream_iterator<string>());

				if (v[0] == "insert")
				{
					if (tree.insert(stoi(v[1]), stoi(v[2])))
						result += "true\n";
					else
						result += "false\n";
				}
				if (v[0] == "find")
				{
					string str = tree.find(stoi(v[1]));
					if (str == " ")
						result += "null\n";
					else
						result += str + "\n";
				}
			}
			write_result(path_to_output, result);
		}
		else
		{
			cout << "Incorrect number of parameters";
		}
	}
	catch (exception ex)
	{
		cout << ex.what();
	}

}