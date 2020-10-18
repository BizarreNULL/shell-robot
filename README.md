<p align="center">
<a href="https://github.com/BizarreNULL/mobile-offsec/">
  <img src="./assets/logo.png" width="350" />
</a>
</p>
<h1 align="center">
  Shell Robot - <code>template2shell</code>
</h1>

<p align="center">
  Check available templates on <a href="https://github.com/BizarreNULL/shell-robot-templates">shell-robot-templates</a>.
  <br/><br/>
  <a href="http://www.wtfpl.net/txt/copying/">
    <img alt="WTFPL License" src="https://img.shields.io/github/license/BizarreNULL/shell-robot" />
  </a>
  <img alt="GitHub code size in bytes" src="https://img.shields.io/github/languages/code-size/BizarreNULL/shell-robot">
</p>


## Why?

Playing CTFs or even on red teaming workflow, is necessary to memorize a huge collection of reverse shell templates (one-liners or not) for PHP, Linux, macOS, or based in some chaining with some tools. The time lost writing a *revshell* can be the same to lose the time window for persisting access.

The ShellRobot library exposes a very simple API (check the code snippet bellow) to automate the creation of any kind of reverse shell (or things that can be put on our template configuration file), and as a dotnet Core 3.X library, can be integrated on any dotnet program, like CLIs or REST APIs for automatic *revshells* based in some parameters.

```csharp
var shellRobot = new ShellRobot();
await shellRobot.UpdateAsync();

var revshell = await ParseTemplateAsync("Some random complex revshell", new[]
{
    "param1",
    "value of param1",
    "param2",
    "..."
});
```



## Roadmap

I started the project for learning a bit more about C# 9.0, but as the maturity of `ShellRobot` library is groing, maybe someday can turn in a really useful thing:

- [x] **0x00**: Make the satuday project work;
- [ ] **0x01**: Documentation of `.json` template configuration (check example [here](https://github.com/BizarreNULL/shell-robot-templates/blob/main/bash-tcp.json));
- [ ] **0x02**: Create GitHub Action for validate new PRs with new templates on [this](https://github.com/BizarreNULL/shell-robot-templates) _repo_;
- [ ] **0x03**: Create GitHub Action for CD of the library on NuGet;
- [ ] **0x04**: Make a CLI (or webapi) consuming the `ShellRobot` library and community templates. 



## Licenses

[Shell Robot](https://github.com/BizarreNULL/shell-robot) project icons made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com. The source code is licensed under [WTFPL](http://www.wtfpl.net/).</a>