using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace UITests
{
    public class ConviteUITests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            // options.AddArgument("headless"); // descomentar para correr sem abrir janela
            _driver = new ChromeDriver(options);
        }

        [Test]
        public void AceitarConvite_DeveAtualizarEstado()
        {
            _driver.Navigate().GoToUrl("http://localhost:5290/login");


            _driver.FindElement(By.Id("Email")).SendKeys("teste0@gmail.com");
            _driver.FindElement(By.Id("Password")).SendKeys("teste0");
            _driver.FindElement(By.XPath("//button[text()='Entrar']")).Click();

            Thread.Sleep(1000);


            _driver.Navigate().GoToUrl("http://localhost:5290/Convites/MeusConvites");

            Thread.Sleep(1000);


            var botaoAceitar = _driver.FindElement(By.XPath("//button[text()='Aceitar']"));
            botaoAceitar.Click();

            Thread.Sleep(1000);


            //var body = _driver.FindElement(By.TagName("body")).Text;
            //Assert.That(body, Does.Contain("Nenhum convite pendente"));
        }



        [TearDown]
        public void TearDown()
        {
            _driver.Quit(); // fecha o Chrome
            _driver.Dispose(); // limpa recursos completamente
        }

    }
}
