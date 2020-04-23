# 什么是PO文件？
PO 是 Portable Object (可移植对象)的缩写形式；MO 是 Machine Object (机器对象) 的缩写形式。 是Linux系统中软件本地化常用文件格式，也是WordPress的本地化文件格式，用途比较广泛。
PO是文本格式的文件，源自GNU的gettext()
[https://www.gnu.org/software/gettext/](https://www.gnu.org/software/gettext/) 通过编译生产MO文件(Machine Object)共软件使用。
## gettext简介

通常，程序以英语编写和记录，并在执行时使用英语与用户进行交互。不仅在GNU内部，而且在大量专有和自由软件中，都是如此。使用通用语言在来自所有国家的开发人员，维护人员和用户之间进行交流非常方便。另一方面，大多数人对英语的适应程度不如对自己的母语适应，并且宁愿尽可能多地使用母语进行日常工作。许多人只是希望看到自己的计算机屏幕上显示的英语少得多，而显示的语言多得多。

GNU gettext是GNU Translation Project的重要步骤，因为它是我们可以构建许多其他步骤的资产。该软件包为程序员，翻译人员甚至用户提供了一套完善的工具和文档集。具体来说，GNU gettext实用程序是一组工具，提供了一个框架来帮助其他GNU软件包生成多语言消息。这些工具包括一组有关应如何编写程序以支持消息目录的约定，用于消息目录本身的目录和文件命名组织，支持检索已翻译消息的运行时库以及一些可在其中进行消息处理的独立程序。套可翻译字符串或已翻译字符串的各种方式。特殊的GNU Emacs模式还可以帮助感兴趣的各方准备这些集合或使它们更新。

Windows 版在[https://mlocati.github.io/articles/gettext-iconv-windows.html](https://mlocati.github.io/articles/gettext-iconv-windows.html) 下载 by Michele Locati

源码：
[https://github.com/mlocati/gettext-iconv-windows](https://github.com/mlocati/gettext-iconv-windows)

## PO文件的文件格式
GNU中规定的PO规范：
```bash
white-space
#  translator-comments
#. extracted-comments
#: reference…
#, flag…
#| msgid previous-untranslated-string
msgid untranslated-string
msgstr translated-string
```
其中规定``#``开始的行为注释
``msgid`` 开头，后接空格，之后是引号包裹的行为原文文字
``msgid`` 行的下一行以``msgstr``开头，后接空格，之后是引号包裹的行为译文文字内容
例如：
```bash
#: lib/error.c:116
msgid "Unknown system error"
msgstr "Error desconegut del sistema"
```
在Windows系统中PO文件通过GNU gettext工具中的``msgfmt.exe`` 编译生成MO文件Machine Object；同样MO文件也可以通过工具中的``msgunfmt.exe``反编译回PO文件。

PO文件可以使用PO Edit编辑和处理
[https://poedit.net/](https://poedit.net/)


我认为由于PO文件历史悠久，设计古老，本身存在设计不完善，存在一定缺陷，例如:

 - ``msgid``中提到id然而并没有使用ID作为识别而是使用源语言文字作为id，这样一来，如果原文中存在拼写错误，是不能修正的，如果修改原文则将无法匹配id
 - 仅定义了引号作为原文或者译文的开始和结束，但是并没有定义句子中出现引号如何处理，通常按照标准是需要转义的，需要定义转义字符和转移标准例如``\"``或``""``
 - ..
 
 横向对比xml:xliff设计就很优秀，解决了PO中的问题
```xml 
<trans-unit id='hi'>
	<source>Hello world</source>
	<target>Bonjour le monde</target>
</trans-unit>
 ```
定义了 id，source，target 三个要素，以id作为Unique Value识别和检索,
这样source就是可以变更的了。
并且xliff基于XML文件格式，可以使用xml的全部规则，包括注释，详见：
 [http://docs.oasis-open.org/xliff/xliff-core/xliff-core.html](http://docs.oasis-open.org/xliff/xliff-core/xliff-core.html)
但是正是因为xliff的xml性，却不符合现今的时代标准，现在流行的趋势是去xml化，例如json， 但是目前来看json的格式过于松散并不能替代xliff


正因为GNU PO的局限性，一些企业和组织在GNU的基础上扩展了自定义的PO，例如：

```bash
#: tmp/includes/views/boost.inc:9
#: tmp/includes/panels/boost.inc:9
msgid "HELP_WIFI_BLOCK_LIST"
msgstr ""
"When blocked list is on, blocked devices will not be able to connect to your "
"hotspot even if they have your WiFi password."
```

文件解析：

``#``开始的行为注释
~~``msgid`` 开头，后接空格，之后是引号包裹的行为原文文字~~ 
变更为
``msgid`` 开头，后接空格，之后是引号包裹的ID
~~``msgid`` 行的下一行以``msgstr``开头，后接空格，之后是引号包裹的行为译文文字内容~~ 
变更为
``msgid`` 行的下一行以``msgstr``开头，后接空格，之后是引号包裹的行为原文文字内容
msgstr可以多行排列，以一个空行或者只包含一个``.``的行标志``msgstr``段的结束

翻译后译文替代到``msgstr``段，原文被抹去

对于这种自定义的扩展文件类型，显然原有的编辑器，解析器不能处理，需要自定义解析器，接下来自定义解析器的要点说明：


