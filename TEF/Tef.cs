using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gbot_XamarinAndroid.Printer;
using Gbot_XamarinAndroid.SAT.SatPages;
using Java.Util;
using Java.Util.Regex;
using Org.Json;

namespace Gbot_XamarinAndroid.TEF
{
    [Activity(Label = "Tef")]
    public class Tef : Activity
    {

        public static string acao = "venda";

        private System.String current = "";
        private string Value = "";
        private Locale mLocale = new Locale("pt", "BR");

        private static int REQ_CODE = 4713;
        private static System.Random random = new System.Random();
        private string op = random.Next(99999).ToString();
        private string currentDateTimeString;
        private string currentDateTimeStringT;

        Venda venda = new Venda();
        RetornoMsiTef retornoSitef = new RetornoMsiTef();

        private Button btnEnviarTransacao, btnCancelaTransacao, btnFuncoes, btnReimpressao;
        //private CheckBox btnValidaImpressao;
        private RadioButton rbCredito, rbDebito, rbTodos, rbParcLoja, rbParcAdm;
        private EditText ipEdit, valPag, qtdeParcelas;
        private TextView txtCupom;

        public Result RESULT_OK { get; private set; }

        public static ConfigPrint configPrint;
        public Context context;
        public static GertecPrinter printer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Tef);

            InitEditText();
            InitRadioButtons();
            InitButtons();

            valPag.Text = "10,00";
            //rbMsitef.Checked = true;
            rbParcAdm.Checked = true;
            //btnValidaImpressao.Checked = false;
           // btnValidaImpressao.Enabled = false;
            rbCredito.Checked = true;

            currentDateTimeString = DateTime.Now.ToString("dd/MM/yyyy");

            currentDateTimeStringT = DateTime.Now.ToString("HHmmss");

            valPag.TextChanged += HandleTextChanged;

            void HandleTextChanged(object sender, Android.Text.TextChangedEventArgs e)
            {
                Value = valPag.Text;
                if (Value != current)
                {
                    valPag.TextChanged -= HandleTextChanged;
                    string cleanString = Regex.Replace(Value, "[^0-9a-zA-Z]+", "");
                    double parsed = System.Double.Parse(cleanString);
                    Locale teste = new Locale("pt", "BR");
                    var formatted = Java.Text.NumberFormat.GetCurrencyInstance(teste).Format((parsed / 100)).Remove(0, 2);
                    current = formatted;
                    valPag.Text = formatted;
                    valPag.TextChanged += HandleTextChanged;
                    valPag.SetSelection(formatted.Length);
                }
            }

            context = ApplicationContext;
            printer = new GertecPrinter(context);
            configPrint = new ConfigPrint();
            printer.setConfigImpressao(configPrint);

            rbDebito.CheckedChange += (s, e) =>
            {
                if (rbTodos.Checked || rbDebito.Checked)
                {
                    qtdeParcelas.Text = ("1");
                    qtdeParcelas.Enabled = (false);
                }
                else
                {
                    qtdeParcelas.Enabled = (true);
                }
            };

            rbTodos.CheckedChange += (s, e) =>
            {
                if (rbTodos.Checked || rbDebito.Checked)
                {
                    qtdeParcelas.Text = ("1");
                    qtdeParcelas.Enabled = (false);
                }
                else
                {
                    qtdeParcelas.Enabled = (true);
                }
            };

            //rbMsitef.CheckedChange += (s, e) =>
            //{
            //    if (rbMsitef.Checked == true)
            //    {
            //        ipEdit.Enabled = true;
            //        btnValidaImpressao.Checked = false;
            //        btnValidaImpressao.Enabled = false;
            //    }
            //    else
            //    {
            //        ipEdit.Enabled = false;
            //        btnValidaImpressao.Enabled = true;
            //        btnValidaImpressao.Checked = true;
            //    }
            //};

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public string respSitefToJson(Intent data)
        {
            JSONObject json = new JSONObject();
            json.Put("CODRESP", data.GetStringExtra("CODRESP"));
            json.Put("COMP_DADOS_CONF", data.GetStringExtra("COMP_DADOS_CONF"));
            json.Put("CODTRANS", data.GetStringExtra("CODTRANS"));
            json.Put("VLTROCO", data.GetStringExtra("VLTROCO"));
            json.Put("REDE_AUT", data.GetStringExtra("REDE_AUT"));
            json.Put("BANDEIRA", data.GetStringExtra("BANDEIRA"));
            json.Put("NSU_SITEF", data.GetStringExtra("NSU_SITEF"));
            json.Put("NSU_HOST", data.GetStringExtra("NSU_HOST"));
            json.Put("COD_AUTORIZACAO", data.GetStringExtra("COD_AUTORIZACAO"));
            json.Put("NUM_PARC", data.GetStringExtra("NUM_PARC"));
            json.Put("TIPO_PARC", data.GetStringExtra("TIPO_PARC"));
            json.Put("VIA_ESTABELECIMENTO", data.GetStringExtra("VIA_ESTABELECIMENTO"));
            json.Put("VIA_CLIENTE", data.GetStringExtra("VIA_CLIENTE"));
            return json.ToString();
        }

        private void InitEditText()
        {
            valPag = FindViewById<EditText>(Resource.Id.editValorPagamento);
            qtdeParcelas = FindViewById<EditText>(Resource.Id.qtdeParcelas);
            ipEdit = FindViewById<EditText>(Resource.Id.ipEdit);
        }

        private void InitRadioButtons()
        {
            //btnValidaImpressao = FindViewById<CheckBox>(Resource.Id.cbImpressao);
            rbCredito = FindViewById<RadioButton>(Resource.Id.rbCredito);
            rbDebito = FindViewById<RadioButton>(Resource.Id.rbDebito);
            rbTodos = FindViewById<RadioButton>(Resource.Id.rbTodos);

            rbParcAdm = FindViewById<RadioButton>(Resource.Id.rbParcAdm);
            rbParcLoja = FindViewById<RadioButton>(Resource.Id.rbParcLoja);

            //rbMsitef = FindViewById<RadioButton>(Resource.Id.rbMsitef);
            //rbGer7 = FindViewById<RadioButton>(Resource.Id.rbGer7);
        }

        private void InitButtons()
        {
            btnEnviarTransacao = FindViewById<Button>(Resource.Id.btnTransacao);
            btnCancelaTransacao = FindViewById<Button>(Resource.Id.btnCancelar);
            btnFuncoes = FindViewById<Button>(Resource.Id.btnFuncoes);
            btnReimpressao = FindViewById<Button>(Resource.Id.btnReipressao);

            btnEnviarTransacao.Click += delegate
            {
                enviarTransacao();
            };

            btnCancelaTransacao.Click += delegate
            {
                cancelarTransacao();
            };

            btnFuncoes.Click += delegate
            {
                funcoes();
            };

            btnReimpressao.Click += delegate
            {
                reimpressao();
            };
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == REQ_CODE && resultCode == Result.Ok)
            {
                if (data.GetStringExtra("CODRESP") == "0")
                {
                    string impressao = "";
                    // Verifica se tem algo pra imprimir
                    if (!String.IsNullOrEmpty(data.GetStringExtra("VIA_CLIENTE")))
                    {
                        impressao += data.GetStringExtra("VIA_CLIENTE");
                    }
                    if (!string.IsNullOrEmpty(data.GetStringExtra("VIA_ESTABELECIMENTO")))
                    {
                        impressao += "\n\n-----------------------------     \n";
                        impressao += data.GetStringExtra("VIA_ESTABELECIMENTO");
                    }
                    if (!String.IsNullOrEmpty(impressao))
                    {
                        dialogImpressaoGPOS(impressao, 17);
                    }

                }
                // Verifica se ocorreu um erro durante venda ou cancelamento
                if (acao.Equals("venda") || acao.Equals("cancelamento"))
                {
                    if (String.IsNullOrEmpty(data.GetStringExtra("CODRESP")) || !(data.GetStringExtra("CODRESP") == "0"))
                    {
                        dialodTransacaoNegadaMsitef(data);
                    }
                    else
                    {
                        dialodTransacaoAprovadaMsitef(data);
                    }
                }
            }
            else
            {
                // ocorreu um erro
                if (acao == "venda" || acao == "cancelamento")
                {
                    dialodTransacaoNegadaMsitef(data);
                }
            }

        }

        private void dialodTransacaoAprovadaMsitef(Intent data)
        {
            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();

            StringBuilder cupom = new StringBuilder();

            cupom.Append("CODRESP: " + data.GetStringExtra("CODRESP") + "\n");
            cupom.Append("COMP_DADOS_CONF: " + data.GetStringExtra("COMP_DADOS_CONF") + "\n");
            cupom.Append("CODTRANS: " + data.GetStringExtra("CODTRANS") + "\n");

            cupom.Append("CODTRANS: " + data.GetStringExtra("CODTRANS") + " " + Convert.ToString(retornaTipoParcelamento(Convert.ToInt32(data.GetStringExtra("TIPO_PARC")))) + "\n");

            cupom.Append("VLTROCO: " + data.GetStringExtra("VLTROCO") + "\n");
            cupom.Append("REDE_AUT: " + data.GetStringExtra("REDE_AUT") + "\n");
            cupom.Append("BANDEIRA: " + data.GetStringExtra("BANDEIRA") + "\n");

            cupom.Append("NSU_SITEF: " + data.GetStringExtra("NSU_SITEF") + "\n");
            cupom.Append("NSU_HOST: " + data.GetStringExtra("NSU_HOST") + "\n");
            cupom.Append("COD_AUTORIZACAO: " + data.GetStringExtra("COD_AUTORIZACAO") + "\n");
            cupom.Append("NUM_PARC: " + data.GetStringExtra("NUM_PARC") + "\n");

            alertDialog.SetTitle("Ação executada com sucesso");
            alertDialog.SetMessage(cupom.ToString());
            alertDialog.SetButton("OK", delegate
            {
                alertDialog.Dismiss();
            });
            alertDialog.Show();
        }

        private string retornaTipoParcelamento(int op)
        {
            switch (op)
            {
                case 0:
                    return "A vista";
                case 1:
                    return "Pré-Datado";
                case 2:
                    return "Parcelado Loja";
                case 3:
                    return "Parcelado Adm";
                default:
                    return "Valor invalido";
            };
        }

        private void dialodTransacaoNegadaMsitef(Intent data)
        {
            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();

            StringBuilder cupom = new StringBuilder();

            cupom.Append("CODRESP: " + data.GetStringExtra("CODRESP") + "\n");

            alertDialog.SetTitle("Ocorreu um erro durante a realização da ação");
            alertDialog.SetMessage(cupom.ToString());
            alertDialog.SetButton("OK", delegate
            {
                alertDialog.Dismiss();
            });
            alertDialog.Show();
        }

        private void dialogImpressaoGPOS(String texto, int size)
        {
            Console.WriteLine("Texto: " + texto.IndexOf("\n"));
            AlertDialog alertDialog = new AlertDialog.Builder(this).Create();
            StringBuilder cupom = new StringBuilder();
            cupom.Append("Deseja realizar a impressão pela aplicação?");
            alertDialog.SetTitle("Realizar Impressão");
            alertDialog.SetMessage(cupom.ToString());
            alertDialog.SetButton("Sim", OkAction);
            alertDialog.SetButton2("Não", CancelAction);
            alertDialog.Show();

            void OkAction(object sender, DialogClickEventArgs e)
            {
                String textoEstabelecimento = "";
                String textoCliente = "";

                //configPrint.Alinhamento = ("LEFT");
                //configPrint.Fonte = ("MONOSPACE");
                //configPrint.Tamanho = (size);
                //configPrint.Negrito = (true);
                //configPrint.Italico = (false);
                //configPrint.SubLinhado = (false);
                try
                {
                    printer.getStatusImpressora();
                    if (printer.IsImpressoraOK())
                    {
                        printer.setConfigImpressao(configPrint);
                        //Console.WriteLine("GertecPrinter" + printer);                       
                        
                        printer.ImprimeTexto(texto);
                        
                        printer.AvancaLinha(150);
                    }
                    printer.ImpressoraOutput();
                }
                catch (System.Exception)
                {
                    Console.WriteLine(System.Environment.StackTrace);
                }
            }

            void CancelAction(object sender, DialogClickEventArgs e)
            {

                //não faz nada
                Console.WriteLine("Não faz nada aqui");
            }
        }

        private void reimpressao()
        {
            acao = "reimpressao";

            if (Mask.Unmask(valPag.Text).Equals("0,00"))
            {
                dialogoErro("O valor de venda digitado deve ser maior que 0");
            }
            else if (validaIp(ipEdit.Text) == false)
            {
                dialogoErro("Digite um IP válido");
            }
            else
            {
                execulteSTefReimpressao();
            }
        }

        private void execulteSTefReimpressao()
        {
            REQ_CODE = 4321;
            Intent intentSitef = new Intent("br.com.softwareexpress.sitef.msitef.ACTIVITY_CLISITEF");

            intentSitef.PutExtra("empresaSitef", "00000000");
            intentSitef.PutExtra("enderecoSitef", ipEdit.Text);
            intentSitef.PutExtra("operador", "0001");
            intentSitef.PutExtra("data", currentDateTimeString);
            intentSitef.PutExtra("hora", currentDateTimeStringT);
            intentSitef.PutExtra("numeroCupom", op);

            intentSitef.PutExtra("valor", Mask.Unmask(valPag.Text.Replace(",", "").Replace(".", "")));
            intentSitef.PutExtra("CNPJ_CPF", "03654119000176");
            intentSitef.PutExtra("comExterna", "0");

            intentSitef.PutExtra("modalidade", "114");

            intentSitef.PutExtra("isDoubleValidation", "0");
            intentSitef.PutExtra("caminhoCertificadoCA", "ca_cert_perm");
            intentSitef.PutExtra("comprovante", "1");
            /*if (btnValidaImpressao.Checked)
            {
                
            }
            else
            {
                intentSitef.PutExtra("comprovante", "0");
            }*/

            StartActivityForResult(intentSitef, REQ_CODE);
        }

        private void funcoes()
        {
            acao = "funcoes";
            if (Mask.Unmask(valPag.Text).Equals("0,00"))
            {
                dialogoErro("O valor de venda digitado deve ser maior que 0");
            }
            else if (validaIp(ipEdit.Text) == false)
            {
                dialogoErro("Digite um IP válido");
            }
            else
            {
                execulteSTefFuncoes();
            }
        }

        private void execulteSTefFuncoes()
        {
            REQ_CODE = 4321;
            Intent intentSitef = new Intent("br.com.softwareexpress.sitef.msitef.ACTIVITY_CLISITEF");

            intentSitef.PutExtra("empresaSitef", "00000000");
            intentSitef.PutExtra("enderecoSitef", ipEdit.Text);
            intentSitef.PutExtra("operador", "0001");
            intentSitef.PutExtra("data", currentDateTimeString);
            intentSitef.PutExtra("hora", currentDateTimeStringT);
            intentSitef.PutExtra("numeroCupom", op);

            intentSitef.PutExtra("valor", Mask.Unmask(valPag.Text.Replace(",", "").Replace(".", "")));
            intentSitef.PutExtra("CNPJ_CPF", "03654119000176");
            intentSitef.PutExtra("comExterna", "0");

            intentSitef.PutExtra("isDoubleValidation", "0");
            intentSitef.PutExtra("caminhoCertificadoCA", "ca_cert_perm");
            intentSitef.PutExtra("comprovante", "1");
            /*if (btnValidaImpressao.Checked)
            {
                
            }
            else
            {
                intentSitef.PutExtra("comprovante", "0");
            }*/

            intentSitef.PutExtra("modalidade", "110");
            intentSitef.PutExtra("restricoes", "transacoesHabilitadas=16;26;27");

            StartActivityForResult(intentSitef, REQ_CODE);
        }

        private void cancelarTransacao()
        {
            acao = "cancelamento";
            if (Mask.Unmask(valPag.Text) == "0,00")
            {
                dialogoErro("O valor de venda digitado deve ser maior que 0");
            }
            else if (validaIp(ipEdit.Text) == false)
            {
                dialogoErro("Digite um IP válido");
            }
            else
            {
                execulteSTefCancelamento();
            }
        }

        private void execulteSTefCancelamento()
        {
            REQ_CODE = 4321;
            Intent intentSitef = new Intent("br.com.softwareexpress.sitef.msitef.ACTIVITY_CLISITEF");

            intentSitef.PutExtra("empresaSitef", "00000000");
            intentSitef.PutExtra("enderecoSitef", ipEdit.Text);
            intentSitef.PutExtra("operador", "0001");
            intentSitef.PutExtra("data", currentDateTimeString);
            intentSitef.PutExtra("hora", currentDateTimeStringT);
            intentSitef.PutExtra("numeroCupom", op);

            intentSitef.PutExtra("valor", Mask.Unmask(valPag.Text.Replace(",", "").Replace(".", "")));
            intentSitef.PutExtra("CNPJ_CPF", "03654119000176");
            intentSitef.PutExtra("comExterna", "0");

            intentSitef.PutExtra("modalidade", "200");

            intentSitef.PutExtra("isDoubleValidation", "0");
            intentSitef.PutExtra("caminhoCertificadoCA", "ca_cert_perm");
            intentSitef.PutExtra("comprovante", "1");
            /*if (btnValidaImpressao.Checked)
            {
                
            }
            else
            {
                intentSitef.PutExtra("comprovante", "0");
            }*/

            StartActivityForResult(intentSitef, REQ_CODE);
        }

        private void enviarTransacao()
        {
            acao = "venda";
            if (Mask.Unmask(valPag.Text) == "0,00")
            {
                dialogoErro("O valor de venda digitado deve ser maior que 0");

            }
            else if (validaIp(ipEdit.Text) == false)
            {
                dialogoErro("Digite um IP válido");
            }
            else
            {
                if (rbCredito.Checked && (String.IsNullOrEmpty(qtdeParcelas.Text) || qtdeParcelas.Text == "0"))
                {
                    dialogoErro("É necessário colocar o número de parcelas desejadas (obs.: Opção de compra por crédito marcada)");
                }
                else
                {
                    execulteSTefVenda();
                }
            }
        }

        private void execulteSTefVenda()
        {
            REQ_CODE = 4321;
            Intent intentSitef = new Intent("br.com.softwareexpress.sitef.msitef.ACTIVITY_CLISITEF");

            intentSitef.PutExtra("empresaSitef", "00000000");
            intentSitef.PutExtra("enderecoSitef", ipEdit.Text);
            intentSitef.PutExtra("operador", "0001");
            intentSitef.PutExtra("data", "20200324");
            intentSitef.PutExtra("hora", "130358");
            intentSitef.PutExtra("numeroCupom", op);

            intentSitef.PutExtra("valor", Mask.Unmask(valPag.Text.Replace(",", "").Replace(".", "")));
            intentSitef.PutExtra("CNPJ_CPF", "03654119000176");
            intentSitef.PutExtra("comExterna", "0");

            if (rbCredito.Checked)
            {
                intentSitef.PutExtra("modalidade", "3");
                if (qtdeParcelas.Text == "0" || qtdeParcelas.Text == "1")
                {
                    intentSitef.PutExtra("transacoesHabilitadas", "26");
                }
                else if (rbParcLoja.Checked)
                {
                    // Essa informações habilida o parcelamento Loja
                    intentSitef.PutExtra("transacoesHabilitadas", "27");
                }
                else
                {
                    // Essa informações habilida o parcelamento ADM
                    intentSitef.PutExtra("transacoesHabilitadas", "28");
                }
                intentSitef.PutExtra("numParcelas", qtdeParcelas.Text);
            }

            if (rbDebito.Checked)
            {
                intentSitef.PutExtra("modalidade", "2");
                intentSitef.PutExtra("transacoesHabilitadas", "16");
            }

            if (rbTodos.Checked)
            {
                intentSitef.PutExtra("modalidade", "0");
                intentSitef.PutExtra("restricoes", "transacoesHabilitadas=16");
            }

            intentSitef.PutExtra("isDoubleValidation", "0");
            intentSitef.PutExtra("caminhoCertificadoCA", "ca_cert_perm");
            intentSitef.PutExtra("comprovante", "1");
            /*if (btnValidaImpressao.Checked)
            {
                
            }
            else
            {
                intentSitef.PutExtra("comprovante", "0");
            }*/

            StartActivityForResult(intentSitef, REQ_CODE);
        }

        private void dialogoErro(String msg)
        {
            Android.App.AlertDialog alertDialog = new Android.App.AlertDialog.Builder(this).Create();
            alertDialog.SetTitle("Erro ao executar função");
            alertDialog.SetMessage(msg);
            alertDialog.SetButton("OK", delegate
            {
                //Não existe nenhuma ação
                alertDialog.Dismiss();
            });
            alertDialog.Show();
        }

        bool validaIp(string ipserver)
        {
            Console.WriteLine(ipserver);
            Java.Util.Regex.Pattern p = Java.Util.Regex.Pattern.Compile("^([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\." +
                "([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\." +
                "([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\." +
                "([01]?\\d\\d?|2[0-4]\\d|25[0-5])$");
            Matcher m = p.Matcher(ipserver);
            bool b = m.Matches();
            Console.WriteLine(b);
            return b;
        }
    }
}