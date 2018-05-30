using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskDemoWindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "処理中。。。。";
            textBox1.Refresh();
            Thread.Sleep(5000);
            textBox1.Text += "\r\n終了!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "処理中。。。。";

            Task.Factory.StartNew(() => Thread.Sleep(5000))
                .ContinueWith(t =>
                {
                    textBox1.Text += "\r\n終了!";
                }, 
                TaskScheduler.FromCurrentSynchronizationContext());
            
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "処理中。。。。";

            await Task.Factory.StartNew(() => Thread.Sleep(5000));

            textBox1.Text += "\r\n終了!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "処理中。。。。(スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";

            Task.Factory
                .StartNew(() =>
                {
                    Thread.Sleep(3000);
                    return "非同期の結果1 (スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";
                })
                .ContinueWith(t1 =>
                {
                    textBox1.Text += "\r\n" + t1.Result;

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(3000);
                        return t1.Result + " + 非同期の結果2 (スレッドID：" + Thread.CurrentThread.ManagedThreadId +")";

                    }, CancellationToken.None ,TaskCreationOptions.None, TaskScheduler.Default)
                    .ContinueWith(t2 =>
                    {
                        textBox1.Text += "\r\n" + t2.Result;
                        
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(3000);
                            return t2.Result + " + 非同期の結果3 (スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";

                        }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
                        .ContinueWith(t3 =>
                        {
                            textBox1.Text += "\r\n" + t3.Result;
                            textBox1.Text += "\r\n終了！(スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";
                        }, 
                        TaskScheduler.FromCurrentSynchronizationContext());
                    },
                    TaskScheduler.FromCurrentSynchronizationContext());
                },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "処理中。。。。(スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";

            var result1 = await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                return "非同期の結果1 (スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";
            });

            textBox1.Text += "\r\n" + result1;

            var result2 = await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                return result1 + " + 非同期の結果2 (スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";

            });

            textBox1.Text += "\r\n" + result2;

            var result3 = await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                return result2 + " + 非同期の結果3 (スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";

            });

            textBox1.Text += "\r\n" + result3;
            textBox1.Text += "\r\n終了！(スレッドID：" + Thread.CurrentThread.ManagedThreadId + ")";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var random = new Random();
            button4.BackColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        
    }
}
