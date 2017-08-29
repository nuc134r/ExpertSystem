A basic IDE for a Prolog-inspired logic programming language. Semester project.

# UI

User interface was made to look like Visual Studio's dark skin. Check out animations:

![123](https://user-images.githubusercontent.com/13202642/29819828-f71ccebe-8cca-11e7-9253-cec6ff70f198.gif)

# Logikek language

The task was to partly replicate Prolog language. *Logikek* language invented and is parsed with [Sprache](https://github.com/sprache/Sprache) library. 

Language consisits of three types of clauses.

### Rule

For example we want to create a rule which declares that **a good hobbie** is the one which is **fun**.

```
GoodHobbie(X) : Fun(X);
```

`X` stands for an atom (like atom in Prolog). Atom is like a variable in math formula. Atoms must be one-letter.

### Fact

Let's declare the fact that coding is fun.

```
Fun(Coding);
```

In the rule above we had an identical condition `Fun(X)`.

### Query

Now computer knows that a good hobbie is something fun and that coding is fun. So is coding a good hobbie? Let's find it out.

![image](https://user-images.githubusercontent.com/13202642/29818746-68eb824c-8cc6-11e7-933e-edd1d2bc5bce.png)

Here is more complex example:

![image](https://user-images.githubusercontent.com/13202642/29819112-e4d87846-8cc7-11e7-95b6-9156892825d7.png)
