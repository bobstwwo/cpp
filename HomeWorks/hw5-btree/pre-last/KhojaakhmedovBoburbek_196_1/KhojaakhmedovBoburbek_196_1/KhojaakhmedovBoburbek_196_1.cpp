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

	Node(int t, bool isLeaf) {
		leaf = isLeaf;
		this->t = t;
		C = new Node * [2 * t];
		cntKeys = 0;
		keys = vector<int>(2 * t - 1);
		vals = new int[2 * t - 1];
	}

	void insertNonFull(int k, int val) {
		int i = cntKeys - 1;

		if (leaf) {
			while (i >= 0 && keys[i] > k) {
				keys[i + 1] = keys[i];
				vals[i + 1] = vals[i];
				i--;
			}
			keys[i + 1] = k;
			vals[i + 1] = val;
			cntKeys = cntKeys + 1;
		}
		else {
			while (i >= 0 && keys[i] > k)
				i--;

			if (C[i + 1]->cntKeys == 2 * t - 1) {
				splitChild(i + 1, C[i + 1]);

				if (keys[i + 1] < k)
					i++;
			}
			C[i + 1]->insertNonFull(k, val);
		}
	}
	void splitChild(int i, Node* y) {
		Node* z = new Node(y->t, y->leaf);
		z->cntKeys = t - 1;

		for (int j = 0; j < t - 1; j++)
			z->keys[j] = y->keys[j + t];

		if (y->leaf == false) {
			for (int j = 0; j < t; j++)
				z->C[j] = y->C[j + t];
		}

		y->cntKeys = t - 1;
		for (int j = cntKeys; j >= i + 1; j--)
			C[j + 1] = C[j];

		C[i + 1] = z;

		for (int j = cntKeys - 1; j >= i; j--)
			keys[j + 1] = keys[j];

		keys[i] = y->keys[t - 1];
		cntKeys = cntKeys + 1;
	}
	string find(int k) {
		int i = 0;
		while (cntKeys > i && keys[i] < k)
			i = i + 1;
		if (keys[i] == k)
			return to_string(this->vals[i]);
		else
			return " ";
		if (leaf)
			return " ";
		return C[i]->find(k);
	}

	friend class Tree;
};

class Tree {
	Node* head = NULL;
	int t;

public:

	Tree(int t) {
		this->t = t;
	}

	string find(int k) {
		if (head == 0)
			return " ";
		else
			return head->find(k);
	}
	template<typename C, typename T>
	bool contains(C&& c, T e) {
		return find(begin(c), end(c), e) != end(c);
	};
	bool insert(int k, int val) {
		if ((head != NULL) && (contains(head->keys, k) == 1))
		{
			return false;
		}
		else
		{
			if (head == 0) {
				head = new Node(t, true);
				head->keys[0] = k;
				head->vals[0] = val;
				head->cntKeys = 1;
				return true;
			}
			else {
				if (head->cntKeys == 2 * t - 1)
				{
					Node* s = new Node(t, false);

					s->C[0] = head;

					s->splitChild(0, head);

					int i = 0;
					if (s->keys[0] < k)
						i++;
					s->C[i]->insertNonFull(k, val);

					head = s;
				}
				else
				{
					head->insertNonFull(k, val);
				}
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