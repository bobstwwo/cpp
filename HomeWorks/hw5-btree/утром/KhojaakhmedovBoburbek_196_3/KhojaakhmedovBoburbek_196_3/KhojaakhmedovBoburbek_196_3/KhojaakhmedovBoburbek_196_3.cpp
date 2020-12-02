#include <iostream>
#include <iostream>
#include <math.h>
#include <vector>
#include <string>
#include <fstream>
#include <map>
#include <cmath>
#include <sstream>
using namespace std;

class Node {
	int* keys;
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
		keys = new int[2 * t - 1];
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
		{
			z->keys[j] = y->keys[j + t];
			z->vals[j] = y->vals[j + t];
		}

		if (y->leaf == false) {
			for (int j = 0; j < t; j++)
				z->C[j] = y->C[j + t];
		}

		y->cntKeys = t - 1;
		for (int j = cntKeys; j >= i + 1; j--)
			C[j + 1] = C[j];

		C[i + 1] = z;

		for (int j = cntKeys - 1; j >= i; j--)
		{
			keys[j + 1] = keys[j];
			vals[j + 1] = vals[j];
		}

		keys[i] = y->keys[t - 1];
		vals[i] = y->vals[t - 1];
		cntKeys = cntKeys + 1;
	}
	string find(int k) {
		int i = 0;
		while (cntKeys > i && keys[i] < k)
			i = i + 1;
		if (keys[i] == k)
			return to_string(this->vals[i]);
		if (leaf)
			return " ";
		return C[i]->find(k);
	}

	void borrowFromPrev(int idx) {
		Node* child = C[idx];
		Node* sibling = C[idx - 1];

		for (int i = child->cntKeys - 1; i >= 0; --i)
		{
			child->keys[i + 1] = child->keys[i];
			child->vals[i + 1] = child->vals[i];
		}

		if (!child->leaf) {
			for (int i = child->cntKeys; i >= 0; --i)
				child->C[i + 1] = child->C[i];
		}

		child->keys[0] = keys[idx - 1];
		child->vals[0] = vals[idx - 1];

		if (!child->leaf)
			child->C[0] = sibling->C[sibling->cntKeys];

		keys[idx - 1] = sibling->keys[sibling->cntKeys - 1];
		vals[idx - 1] = sibling->vals[sibling->cntKeys - 1];

		child->cntKeys += 1;
		sibling->cntKeys -= 1;

		return;
	}
	void borrowFromNext(int idx) {
		Node* child = C[idx];
		Node* sibling = C[idx + 1];

		child->keys[(child->cntKeys)] = keys[idx];
		child->vals[(child->cntKeys)] = vals[idx];

		if (!(child->leaf))
			child->C[(child->cntKeys) + 1] = sibling->C[0];

		keys[idx] = sibling->keys[0];
		vals[idx] = sibling->vals[0];

		for (int i = 1; i < sibling->cntKeys; ++i)
		{
			sibling->keys[i - 1] = sibling->keys[i];
			sibling->vals[i - 1] = sibling->vals[i];
		}

		if (!sibling->leaf) {
			for (int i = 1; i <= sibling->cntKeys; ++i)
				sibling->C[i - 1] = sibling->C[i];
		}

		child->cntKeys += 1;
		sibling->cntKeys -= 1;

		return;
	}
	void merge(int idx) {
		Node* child = C[idx];
		Node* sibling = C[idx + 1];

		child->keys[t - 1] = keys[idx];
		child->vals[t - 1] = vals[idx];

		for (int i = 0; i < sibling->cntKeys; ++i)
		{
			child->keys[i + t] = sibling->keys[i];
			child->vals[i + t] = sibling->vals[i];
		}

		if (!child->leaf) {
			for (int i = 0; i <= sibling->cntKeys; ++i)
			{
				child->C[i + t] = sibling->C[i];
			}
		}

		for (int i = idx + 1; i < cntKeys; ++i)
		{
			keys[i - 1] = keys[i];
			vals[i - 1] = vals[i];
		}

		for (int i = idx + 2; i <= cntKeys; ++i)
			C[i - 1] = C[i];

		child->cntKeys += sibling->cntKeys + 1;
		cntKeys--;

		delete (sibling);
		return;
	}
	void fill(int idx) {
		if (idx != 0 && C[idx - 1]->cntKeys >= t)
			borrowFromPrev(idx);

		else if (idx != cntKeys && C[idx + 1]->cntKeys >= t)
			borrowFromNext(idx);

		else {
			if (idx != cntKeys)
				merge(idx);
			else
				merge(idx - 1);
		}
		return;
	}
	int	 findKey(int k) {
		int idx = 0;
		while (idx < cntKeys && keys[idx] < k)
			++idx;
		return idx;
	}
	int removeFromLeaf(int idx) {
		int rez = vals[idx];
		for (int i = idx + 1; i < cntKeys; ++i)
		{
			keys[i - 1] = keys[i];
			vals[i - 1] = vals[i];
		}
		if (idx + 1 == cntKeys)
		{
			keys[idx] = NULL;
			vals[idx] = NULL;
		}
		cntKeys--;

		return rez;
	}
	int getPredecessor(int idx) {
		Node* cur = C[idx];
		while (!cur->leaf)
			cur = cur->C[cur->cntKeys];

		return cur->keys[cur->cntKeys - 1];
	}
	int getSuccessor(int idx) {
		Node* cur = C[idx + 1];
		while (!cur->leaf)
			cur = cur->C[0];

		return cur->keys[0];
	}
	int removeFromNonLeaf(int idx) {
		int k = keys[idx];
		string rez;

		if (C[idx]->cntKeys >= t) {
			int pred = getPredecessor(idx);
			keys[idx] = pred;
			vals[idx] = pred;
			rez = C[idx]->deletion(pred);
		}

		else if (C[idx + 1]->cntKeys >= t) {
			int succ = getSuccessor(idx);
			keys[idx] = succ;
			vals[idx] = succ;
			rez = C[idx + 1]->deletion(succ);
		}

		else {
			merge(idx);
			rez = C[idx]->deletion(k);
		}
		return stoi(rez);
	}
	string deletion(int k) {
		int idx = findKey(k);
		if (idx < cntKeys && keys[idx] == k) {
			if (leaf)
			{
				int rez = removeFromLeaf(idx);
				return to_string(rez);
			}
			else
			{
				int rez = removeFromNonLeaf(idx);
				return to_string(rez);
			}
		}
		else {
			if (leaf) {
				return "null";
			}

			bool flag = ((idx == cntKeys) ? true : false);

			if (C[idx]->cntKeys < t)
				fill(idx);

			if (flag && idx > cntKeys)
				return C[idx - 1]->deletion(k);
			else
				return C[idx]->deletion(k);
		}
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
	int containsArr(int* arr, int el)
	{
		int rez = 0;
		for (int i = 0; i <= sizeof(arr); i++) {
			if (arr[i] == el) {
				rez = 1;
			}
		}
		return rez;
	}
	bool insert(int k, int val) {
		if ((head != NULL) && (containsArr(head->keys, k) == 1))
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
					head->insertNonFull(k, val);
			}
			return true;
		}
	}
	string deletion(int k) {
		if (!head) {
			return "null";
		}

		return head->deletion(k);

		if (head->cntKeys == 0) {
			Node* tmp = head;
			if (head->leaf)
				head = NULL;
			else
				head = head->C[0];

			delete tmp;
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


int main() {

	string path = "../../../input/test1.txt";

	vector<string> data = get_input(path);

	vector<string> oper = vector<string>();
	vector<int> keys = vector<int>();
	vector<int> vals = vector<int>();
	Tree tree(3);

	for (size_t i = 0; i < data.size(); i++)
	{
		istringstream iss(data[i]);
		vector<string> v((istream_iterator<string>(iss)),
			istream_iterator<string>());

		if (v[0] == "insert")
		{
			if (tree.insert(stoi(v[1]), stoi(v[2])))
				cout << "true" << "\n";
			else
				cout << "false" << "\n";
		}
		if (v[0] == "find")
		{
			string str = tree.find(stoi(v[1]));
			if (str == " ")
				cout << "null" << "\n";
			else
				cout << str << "\n";
		}
	}







	//Tree t(3);
	//cout << t.insert(1, 2) << "\n";
	//cout << t.insert(3, 10) << "\n";
	//cout << t.insert(4, 5) << "\n";

	//cout << t.deletion(1);

	//cout << endl;
	//if (t.find(1) == NULL)
	//	cout << "null";
	//else
	//	cout << t.find(1);


}