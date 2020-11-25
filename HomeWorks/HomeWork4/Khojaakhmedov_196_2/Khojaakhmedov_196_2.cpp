#include <iostream>
#include <math.h>
#include <vector>
#include <string>
#include <fstream>
#include <sstream>
#include <map>
#include <cmath>
using namespace std;

class Bitmap
{
private:
	bool* arr;
	int length;
public:
	Bitmap(int n) {
		arr = new bool[n] {false};
		length = n;
	}
	Bitmap()
	{
		arr = new bool[0];
	}
	void set(int index) {
		arr[index] = true;
	}
	int get(int index) {
		return arr[index];
	}
};

static string str_answer = "";
static map<string, Bitmap> mp;
static int watched_videos = 0;
static pair<vector<string>, int> get_input(string path)
{
	int videos = 0;
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
			if (k == 0)
			{
				int space = line.find(" ");
				string left = line.substr(0, space);
				string right = line.substr(space, line.length() - space);
				videos = stoi(right);
				++k;
				continue;
			}
			input_lines.push_back(line);
		}
	}
	else
	{
		throw exception("File not found!");
	}
	return pair<vector<string>, int>(input_lines, videos);
}
static int get_bitmap_lenght(int n)
{
	return round(n * 9.585);
}
static int hash_1(string word, int size)
{
	hash<string> hash;
	return	hash(word) % size;
}
static int hash_2(string user, string video, int size)
{
	hash<string> hash;
	unsigned long h1 = hash(user);
	unsigned long h2 = hash(video);
	int answer = (17 * h1 + 67 * h2) % size;
	if (answer < 0)
		return answer * (-1);
	else
		return answer;

}
static int hash_3(string user, string video, int size)
{
	hash<string> hash;
	unsigned long h1 = hash(user);
	unsigned long h2 = hash(video);
	int answer = (31 * h1 - 13 * h2) % size;
	if (answer < 0)
		return answer * (-1);
	else
		return answer;
}
static void watch(string user, string video, int n) {
	if (watched_videos <= n) {
		if (mp.count(user) == 0) {
			int size = get_bitmap_lenght(n);
			mp[user] = Bitmap(size);
			int h1 = hash_1(user + " " + video, size);
			int h2 = hash_2(user, video, size);
			int h3 = hash_3(user, video, size);
			mp[user].set(h1);
			mp[user].set(h2);
			mp[user].set(h3);
			str_answer += "Ok\n";
		}
		else {
			int size = get_bitmap_lenght(n);
			int h1 = hash_1(user + " " + video, size);
			int h2 = hash_2(user, video, size);
			int h3 = hash_3(user, video, size);
			mp[user].set(h1);
			mp[user].set(h2);
			mp[user].set(h3);
			str_answer += "Ok\n";
		}
		watched_videos++;
	}
	else
	{
		throw new exception("Different number of watch videos cannot overcome the amount of videos, which was listed in the first parament in <Input file>.");
	}
}
static void check(string user, string video, int n) {
	if (mp.count(user) == 0) {
		str_answer += "No\n";
	}
	else
	{
		int size = get_bitmap_lenght(n);
		int h1 = hash_1(user + " " + video, size);
		int h2 = hash_2(user, video, size);
		int h3 = hash_3(user, video, size);
		if (mp[user].get(h1) && mp[user].get(h2) && mp[user].get(h3))
		{
			str_answer += "Probably\n";
		}
		else
		{
			str_answer += "No\n";
		}
	}
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


int main(int argc, char* argv[])
{
	try {
		if (argc == 3) {
			string path_to_input = argv[1];
			string path_to_output = argv[2];
			auto get_vec_num = get_input(path_to_input);
			int numb_videos = get_vec_num.second;
			vector<string> data_input = get_vec_num.first;


			str_answer += "Ok\n";
			for (int i = 0; i < data_input.size(); i++)
			{
				istringstream iss(data_input[i]);
				vector<string> v((istream_iterator<string>(iss)),
					istream_iterator<string>());

				if (v[0] == "watch")
				{
					watch(v[1], v[2], numb_videos);
				}
				if (v[0] == "check")
				{
					check(v[1], v[2], numb_videos);
				}
			}
			write_result(path_to_output, str_answer);
		}
		else
		{
			std::cout << "Incorrect number of parameters";
		}
	}
	catch (exception ex)
	{
		std::cout << ex.what();
	}
}










