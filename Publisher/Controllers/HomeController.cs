﻿using Microsoft.AspNetCore.Mvc;
using Publisher.Models;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;

namespace Publisher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string selectedOption="orange", string message="")
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://wfhmzgqe:1tjIGzKe2uwJT2lAB_RHES42Ch2MnBwG@beaver.rmq.cloudamqp.com/wfhmzgqe")
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_exchange", type: ExchangeType.Topic);

                var routingKey = selectedOption;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "topic_exchange", routingKey: routingKey, basicProperties: null, body: body);
            }

            return View();
        }
    }
}
