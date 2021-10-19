Ignore Content before first chapter
In a future version this could be used as description for the root deck (and 
maybe the name)
In general, ignored content will be logged as warning.

# Chapter 1

Ignore content not associated with a question
In a future version this could be used as description for this chapters' deck

## First Question?

First Answer

## Multiline Answer?

First line
Second line

Third line

## How are new Chapters Generated?

A new chapter is marked by a top-level heading (`# Chapter 1`) which is also the
name of the new chapter.

## How are Questions/Answer Pairs Made?

A question is a level 2 heading (`## Question?`) under a chapter. The
corresponding answer is the content that follows it. The end of the content is
marked by a new chapter, a new question or a separator (`---`).

---

Example ignored content with the use of a separator

# Chapter 2

Some more ignored content.

## 2*4?

8

## How are Formulas Written?

$\alpha * \beta \cdot \frac{n!}{b!}$

## How can I make a table?

Test  | 1   | 4
------|-----|----
n1    | n2  | n4

## How can I make a List?

- First item
- second item

## Picture

![](launch.jpg)

## Picture with Text

Here is a picture from NASA:

![](launch.jpg)

# Todo

## Fix some Formatting Errors

- list contents are displayed center and not aligned with bullet points (maybe
can be fixed with .css settings in anki?)
- maybe allow custom node type settings etc.
