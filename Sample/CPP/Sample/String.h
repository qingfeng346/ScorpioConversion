#ifndef String_h__
#define String_h__
class String
{
protected:
	size_t length;
	char * buffer;
public:
	String(const String &);
	String(const char *);
	String(const char *, size_t );
	~String();
	void Allocate(size_t , bool );
	void Assign(const char * , size_t );
	void SetLength(size_t);
	size_t GetLength() const;
	char * GetBuffer() const;
	void Concatenate(const char *, size_t );
	int Compare(const char *) const ;
	int Compare(const char *, size_t) const;
	int Compare(const String &) const;
	String SubString(size_t , size_t length = -1) const;
	String & operator += (const char *);
	String & operator += (const String &);
	String & operator += (char);
	String & operator = (const char *);
	String & operator = (const String &);
	String & operator = (char);
	const char &operator[](size_t );
	//String Format(const char * format, ...);
	static String Format(const char * format, ...);
};

String operator +(const String &, const char *);
String operator +(const char *, const String &);
String operator +(const String &, const String &);

bool operator ==(const String &, const String &);
bool operator !=(const String &, const String &);

bool operator ==(const String &, const char *);
bool operator !=(const String &, const char *);

bool operator ==(const char *, const String &);
bool operator !=(const char *, const String &);



#endif // String_h__