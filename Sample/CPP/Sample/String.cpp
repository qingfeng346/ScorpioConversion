#include "String.h"
#include "ScorpioConfig.h"
#include <stdarg.h>		// va_list, va_start(), etc
#include <stdlib.h>		// strtod(), strtol()
#include <stdio.h>		// _vsnprintf
#include <string.h>		// some compilers declare memcpy() here

String::String(const String & str) : String(str.buffer, str.length) {

}
String::String(const char * str) : String(str, strlen(str)) {

}
String::String(const char * str, size_t len) {
	length = 0;
	buffer = null;
	Assign(str, len);
}
String::~String() {
	Safe_Free(buffer);
}
int String::Compare(const char * str) const {
	return Compare(str, strlen(str));
}
int String::Compare(const String &str) const {
	return Compare(str.buffer, str.length);
}
int String::Compare(const char * str, size_t len) const {
	if (length == 0) {
		if (str == 0 || len == 0) return 0; // Equal
		return 1; // The other string is larger than this
	}
	if (str == 0) {
		if (length == 0)
			return 0; // Equal
		return -1; // The other string is smaller than this
	}
	if (len < length) {
		int result = memcmp(buffer, str, len);
		if (result == 0) return -1; // The other string is smaller than this
		return result;
	}
	int result = memcmp(buffer, str, length);
	if (result == 0 && length < len) return 1; // The other string is larger than this
	return result;
}
void String::Allocate(size_t len, bool keepData) {
	if (length == 0 || len > length) {
		char * buf = Malloc(char, len + 1);
		if (buf == null) { 
			return; 
		}
		if (buffer && keepData) {
			memcpy(buf, buffer, length);
		}
		Safe_Free(buffer);
		buffer = buf;
	}
	length = len;
	buffer[length] = 0;
}
void String::Concatenate(const char * str, size_t len)
{
	size_t oldLength = length;
	SetLength(length + len);
	memcpy(buffer + oldLength, str, len);
	buffer[length] = 0;
}
void String::Assign(const char *str, size_t len) {
	Allocate(len, false);
	memcpy(buffer, str, length);
	buffer[length] = 0;
}
void String::SetLength(size_t length) {
	Allocate(length, true);
}
size_t String::GetLength() const {
	return length;
}
char * String::GetBuffer() const {
	return buffer;
}
const char & String::operator[](size_t index) {
	return buffer[index];
}
String & String::operator +=(const char * str)
{
	Concatenate(str, strlen(str));
	return *this;
}
String & String::operator+=(const String & str) {
	Concatenate(str.buffer, str.length);
	return *this;
}
String & String::operator +=(char ch) {
	Concatenate(&ch, 1);
	return *this;
}

String & String::operator =(const char * str) {
	size_t len = str ? strlen(str) : 0;
	Assign(str, len);
	return *this;
}
String & String::operator =(const String & str) {
	Assign(str.buffer, str.length);
	return *this;
}
String & String::operator =(char ch) {
	Assign(&ch, 1);
	return *this;
}

String String::SubString(size_t start, size_t len) const
{
	if (start >= length || len == 0)
		return String("");
	if (len > (length - start)) len = length - start;
	return String(buffer + start, len);
}
String String::Format(const char * format, ...) {
	va_list args;
	va_start(args, format);
	int n = 256;
	String str = "";
	while (true) {
		str.Allocate(n, false);
		int r = vsnprintf(str.buffer, n, format, args);
		if (r >= 0) {
			break;
		}
	}
	va_end(args);
	return str;
}

String operator +(const String & a, const String & b)
{
	String res = a;
	res += b;
	return res;
}

String operator +(const char *a, const String &b)
{
	String res = a;
	res += b;
	return res;
}

String operator +(const String &a, const char *b)
{
	String res = a;
	res += b;
	return res;
}
bool operator ==(const String &a, const char *b)
{
	return a.Compare(b) == 0;
}

bool operator !=(const String &a, const char *b)
{
	return a.Compare(b) != 0;
}

bool operator ==(const String &a, const String &b)
{
	return a.Compare(b) == 0;
}

bool operator !=(const String &a, const String &b)
{
	return a.Compare(b) != 0;
}

bool operator ==(const char *a, const String &b)
{
	return b.Compare(a) == 0;
}

bool operator !=(const char *a, const String &b)
{
	return b.Compare(a) != 0;
}