using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendário
{
    public partial class FormFeriados : Form
    {
        public FormFeriados()
        {
            InitializeComponent();
        }
        private double mes,dia;
        private int ano;
        private string extenso(int Ordinal)//Ha venha Nota extraa
        {
            string extenso=string.Empty;
            int j = 8;
            string[] retorno2 = new string[] {" primeiro","  segundo"," terceiro"," quarto"," quinto"," sexto"," sétimo"," oitavo"," nono"};//array para os dias de 0 a 10
            string[] retorno1 = new string[] { "décimo", " vigésimo", "trigésimo", "quadragésimo", "quinquagésimo", "sexagésimo", "septuagésimo", "octogésimo", "nonagésimo" }; //array para dias de 99 a 10
            if (Ordinal >= 300)//para saber se é mair que 300 dias
            {
               extenso = "trecentésimo ";
               Ordinal = Ordinal - 300;      //diminui para que prossiga o numero de dias ja descontados            
            }
            if (Ordinal >= 200)
            {
                extenso = "ducentésimo ";
                Ordinal = Ordinal - 200;
            }
            if (Ordinal >= 100)
            {
                extenso = "centésimo ";
                Ordinal = Ordinal - 100;
            }         
            for (int i = 90; i > 10; i=i-10)//for para rodar quantos das ele tem
            {
               
                if (Ordinal >= i)//ve qual numero está
                {
                    extenso = extenso + retorno1[j];//acrescenta a posição de acordo com o numero de dias
                    Ordinal = Ordinal - i;      
                    break;
                }
                j--;
            }   
            for(int i = 9; i > 0; i=i-1)
            {
               if (Ordinal >= i)
                {
                    extenso = extenso + retorno2[i-1];
                    Ordinal = Ordinal - i;                 
                    break;
                }            
            }
            return extenso;
        }
        private string DiaDaSemana(double ano, double mes, double dia)//Algoritmo Keller
        {
            double A = 0, B, C, D, E, F, G, H, I, J;
            string[] diasemana = new string[] { "Sabado", "Domingo", "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira" };//instancio array com os dias da semana 
            string retorna = string.Empty;
            A = Math.Floor((12.0 - mes) / 10);
            B = ano - A;
            C = mes + (12 * A);
            D = Math.Floor(B / 100);
            E = Math.Floor(D / 4);
            F = E + 2 - D;
            G = Math.Floor(365.25 * B);
            H = Math.Floor(30.6001 * (C + 1));
            I = F + G + H + dia + 5;
            J = I % 7;
            return diasemana[Convert.ToInt32(J)]; // retorno o dia conforme o resto da divisao de J
        }
        private DateTime Pascoa(double ano)//Codificado o algoritmo De meeus/Jones/Butcher
        {
            double a, b, c, d, e, f, g, h, i, k, L, m;
            a = (ano % 19);
            b = Math.Floor(ano / 100);
            c = (ano % 100);
            d = Math.Floor(b / 4);
            e = (b % 4);
            f = Math.Floor((b + 8) / 25);
            g = Math.Floor((1 + b - f) / 3);
            h = (((19 * a) + b + 15 - (d + g)) % 30);
            i = Math.Floor(c / 4);
            k = (c % 4);
            L = ((32 + (2 * e) + (2 * i) - (h + k)) % 7);
            m = Math.Floor((a + (11 * h) + (22 * L)) / 451);
            mes = Math.Floor((h + L - (7 * m) + 114) / 31);
            dia = ((h + L - (7 * m) + 114) % 31) + 1;

            DateTime pascoa = new DateTime(Convert.ToInt16(ano), Convert.ToInt16(mes), Convert.ToInt16(dia));
            return pascoa;// retorna a Datetime
        }
        private Boolean Bissexto(int ano)
        {
            if ((ano % 400 == 0) || (ano % 4 == 0 && ano % 100 != 0))// se ele entrar neste if então será bissexto
                return true;
            else
                return false;
        }
        private int[] GeraTabelaMeses(int ano)
        {
            int[] mes = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };//instancio o array com os dias de cada mes

            if (Bissexto(ano) == true)
            {
                mes[1] = 29; //caso seja bisssexto levara o mes de fevereiro com 29 dias
            }
            return mes;
        }
        private int[] GeraTabelaOrdinais(int ano)
        {
            int[] meses = GeraTabelaMeses(ano);//guarda na variavel o array de meses
            int[] ordinal=new int[12];//cria um novo array com 12 posições          
            int guarda=0,j=0;

            ordinal[0] = 0;//instancio o primeiro elemento com 0

            for (int i = 0; i < 11; i++)//porque de ser 11 porque ele caminha até a posição 11 no caso o J
            {
                j = i + 1; // para que ele caia na posição seguinte acrescentei 1 
                guarda = guarda + meses[i]; //faço a soma de acordo com os dias de cada mes ja considerando ele bissexto ou não
                ordinal[j] = guarda;//e guardo os valores no array
            }
            return ordinal;
        }
        private int DataParaOrdinal(int ano, int mes, int dia)
        { int[] ordinal = GeraTabelaOrdinais(ano);//guardo o array em uma variavel
            
            return ordinal[mes - 1] + dia; ;//retorno o número de dias
        }           
        private DateTime OrdinalParaData(int diaOrdinal, int ano)
        {
            int[] ordinais = GeraTabelaOrdinais(ano);//guardo em uma variavel 
            int mes=0, dia=0;
           
            for (int i = 0; i <= 10; i++)
            {
                if (ordinais[i + 1] >= diaOrdinal)//vejo em qual posição ele deve fazer a subtração da fórmula 
                {
                    dia =  diaOrdinal -ordinais[i];//subtraio  ex: dia ordinal 3 então sera 3-0
                    mes = i + 1;//o mes é sempre um a mais do que ex: janeiro é 0 então é 0+1=1
                    break;//para parar o for e ir pra o próximo                   
                }           
            }       
            DateTime data = new DateTime(ano, mes, dia);
            return data;
        }
            private void buttonCalcular_Click(object sender, EventArgs e)
        {
            if (maskedTextBoxAno.Text == string.Empty)//Retorna caso esteja vazio
                return;
           
            ano = Convert.ToInt16(maskedTextBoxAno.Text);//converte o ano para inteiro

            if (ano < 1587)//if necessário para o calculo ser exato
            {
                MessageBox.Show("Os feriados só podem ser corretamente calculados a partir de 1587: ", "ERRO");
                return;
            }
            richTextBox1.Clear();
          
           
            DateTime pascoa = Pascoa(ano);

            richTextBox1.AppendText("Confraternização universal: " + DiaDaSemana(ano,01, 01) + " , " + new DateTime(ano,01,01).ToShortDateString() + Environment.NewLine);

            int CarnavalOrdinal = DataParaOrdinal(pascoa.Year, pascoa.Month, pascoa.Day) - 47;
            DateTime carnaval = OrdinalParaData(CarnavalOrdinal, ano);
            richTextBox1.AppendText("Carnaval: " + DiaDaSemana(carnaval.Year, carnaval.Month, carnaval.Day) + " , " + carnaval.ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Dia da mulher: " + DiaDaSemana(ano, 03, 08) + " , " + new DateTime(ano, 03, 08).ToShortDateString() + Environment.NewLine);

            int sextaSantaOrdinal = DataParaOrdinal(pascoa.Year, pascoa.Month, pascoa.Day) - 2;
            DateTime sextaSanta = OrdinalParaData(sextaSantaOrdinal, ano);
            richTextBox1.AppendText("Paixão de cristo: " + DiaDaSemana(sextaSanta.Year, sextaSanta.Month, sextaSanta.Day) + " , " + sextaSanta.ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Pascoa: " + DiaDaSemana(pascoa.Year, pascoa.Month, pascoa.Day) + " , " + pascoa.ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Tiradetes: " + DiaDaSemana(ano, 04, 21) + " , " + new DateTime(ano, 04, 21).ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Dia do trabalho: " + DiaDaSemana(ano, 05, 01) + " , " + new DateTime(ano, 05, 01).ToShortDateString() + Environment.NewLine);

            int CorpusChristiOrdinal = DataParaOrdinal(pascoa.Year, pascoa.Month, pascoa.Day) + 60;
            DateTime CorpusChristi = OrdinalParaData(CorpusChristiOrdinal, ano);
            richTextBox1.AppendText("Corpus Christi: " + DiaDaSemana(CorpusChristi.Year, CorpusChristi.Month, CorpusChristi.Day) + " , " + CorpusChristi.ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Dia da Independência do Brasil: " + DiaDaSemana(ano, 09, 07) + " , " + new DateTime(ano, 09, 07).ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Dia de Nossa Senhora Aparecida: " + DiaDaSemana(ano, 10, 12) + " , " + new DateTime(ano, 10, 12).ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Dia dos Finados: " + DiaDaSemana(ano, 11, 02) + " , " + new DateTime(ano, 11, 02).ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Dia da Proclamação da República: " + DiaDaSemana(ano, 11, 15) + " , " + new DateTime(ano, 11, 15).ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText("Natal, Nascimento de Jesus: " + DiaDaSemana(ano, 12, 25) + " , " + new DateTime(ano, 12, 25).ToShortDateString() + Environment.NewLine);

            richTextBox1.AppendText(Environment.NewLine+"Nota EXTRA"+Environment.NewLine);

            richTextBox1.AppendText("Natal: É O " +extenso(DataParaOrdinal(ano, 12, 25)) + " Dia do ano " +Environment.NewLine);

            richTextBox1.AppendText("Confraternização universal: É O " + extenso(DataParaOrdinal(ano, 01, 01)) + " Dia do ano " + Environment.NewLine);

            richTextBox1.AppendText("Páscoa: É O " + extenso(DataParaOrdinal(pascoa.Year, pascoa.Month, pascoa.Day)) + " Dia do ano " + Environment.NewLine);

        }
    }
}
