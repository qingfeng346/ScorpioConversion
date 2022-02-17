from setuptools import setup, find_packages
f = open("version", encoding="utf8")
version = f.read()
f.close()
setup (
    name="ScorpioConversionRuntime",
    version=version,
    author="while",
    author_email="qingfeng346@outlook.com",
    description="https://github.com/qingfeng346/ScorpioConversion\npython基础运行库",
    url="https://github.com/qingfeng346/ScorpioConversion",
    packages=find_packages()
)