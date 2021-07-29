using System;
using System.Collections.Generic;
using System.IO;

namespace Controle_Clientes
{
    class Program
    {
        // Definindo a struct Cliente, que representará o tipo cliente do programa
        struct Cliente
        {
            public int codigo;
            public string nome;
            public string email;
            public string cpf;
        }

        // Marcação do último código usado
        static int ultimoCodigo;

        static List<Cliente> clientes = new List<Cliente>();

        // Definindo as opções do Menu
        enum Menu { LISTAR = 1, ADICIONAR, EXCLUIR, SAIR}

        /* Função Carregar
         * Carrega os dados do arquivo em que os clientes estão sendo salvos e os salva na lista de Clientes
         */
        static void Carregar()
        {
            try
            {
                StreamReader stream = new StreamReader("clientes.dat");
                Cliente cliente = new Cliente();
                // Lendo o arquivo
                string linha = stream.ReadLine();
                while (linha != null)
                {
                    ultimoCodigo = int.Parse(linha);
                    cliente.codigo = ultimoCodigo;
                    linha = stream.ReadLine();
                    cliente.nome = linha;
                    linha = stream.ReadLine();
                    cliente.cpf = linha;
                    linha = stream.ReadLine();
                    cliente.email = linha;
                    linha = stream.ReadLine();
                    clientes.Add(cliente);
                }
                stream.Close();
            }
            catch (Exception e)
            {
                ultimoCodigo = 0;
                Console.WriteLine("Inicializando base de dados.");
                Console.WriteLine("Aperte Enter para continuar");
                Console.ReadLine();
                Console.Clear();
            }

        }

        /* Função Salvar
         * Salva os dados do programa no arquivo em que os clientes estão sendo salvos
         */
        static void Salvar()
        {
            StreamWriter stream = new StreamWriter("clientes.dat");
            foreach(Cliente cliente in clientes)
            {
                stream.WriteLine(cliente.codigo);
                stream.WriteLine(cliente.nome);
                stream.WriteLine(cliente.cpf);
                stream.WriteLine(cliente.email);
            }
            stream.Close();
        }

        /* Função Entrada
         * Responsável por ler a entrada do usuário e verificar se ela é um número inteiro e se ela se enquadra nas opções disponíveis.
         * Parâmetro: uma string contendo a mensagem a ser exibida para o usuário solicitando que digite.
         * Retorno: int contendo a opção escolhida pelo usuário*/
        static int Entrada(string mensagem)
        {
            do
            {
                string entrada;
                int num;
                Console.Write(mensagem);
                entrada = Console.ReadLine();
                if (int.TryParse(entrada, out num) && num > 0 && num < 5)
                {
                    return num;
                }
                else
                {
                    Console.WriteLine("Valor informado não é válido! Informe outro valor.");
                }
            } while (true);
        }

        /* Função Valida_CPF
         * Responsável por validar o CPF informado durante a adição de clientes
         * Parâmetro: Recebe uma String contendo o CPF informado durante o cadastro.
         */
        static string Valida_CPF(string CPF)
        {
            // Verificando se foram digitados apenas números inteiros
            if (!double.TryParse(CPF, out double num))
            {
                Console.WriteLine("Digite apenas os números do CPF.");
                return null;
            }

            // Verificando se a quantidade de digitos é igual a 11
            if (CPF.Length != 11)
            {
                Console.WriteLine("A quantidade de dígitos digitadas está incorreta. Digite novamente o CPF");
                return null;
            }

            // Quebrando em caracteres a string CPF
            char[] num_CPF = new char[CPF.Length];
            using (StringReader sr = new StringReader(CPF))
            {
                sr.Read(num_CPF, 0, CPF.Length);               
            }

            // Validando se os números são todos iguais (Não existe CPF com todos os digitos iguais)
            if (num_CPF[0] == num_CPF[1] && num_CPF[1] == num_CPF[2] && num_CPF[2] == num_CPF[3] && num_CPF[3] == num_CPF[4] &&
                num_CPF[4] == num_CPF[5] && num_CPF[5] == num_CPF[6] && num_CPF[6] == num_CPF[7] && num_CPF[7] == num_CPF[8] &&
                num_CPF[8] == num_CPF[9] && num_CPF[9] == num_CPF[10])
            {
                Console.WriteLine("Não existem CPFs com todos os dígitos iguais. Digite novamente o CPF.");
                return null;
            }

            // Validando pelo dígito verificador
            // Critério 1º dígito: soma1 := num1 * 10 + num2 * 9 + num3 * 8 + num4 * 7 + num5 * 6 + num6 * 5 + num7 * 4 + num8 * 3 + num9 * 2
            int soma, digito1, digito2;
            soma = (int)Char.GetNumericValue(num_CPF[0]) * 10 +
                (int)Char.GetNumericValue(num_CPF[1]) * 9 +
                (int)Char.GetNumericValue(num_CPF[2]) * 8 +
                (int)Char.GetNumericValue(num_CPF[3]) * 7 +
                (int)Char.GetNumericValue(num_CPF[4]) * 6 +
                (int)Char.GetNumericValue(num_CPF[5]) * 5 +
                (int)Char.GetNumericValue(num_CPF[6]) * 4 +
                (int)Char.GetNumericValue(num_CPF[7]) * 3 +
                (int)Char.GetNumericValue(num_CPF[8]) * 2;
            digito1 = (soma * 10)  % 11;
            if (digito1 == 10)
                digito1 = 0;
            soma = (int)Char.GetNumericValue(num_CPF[0]) * 11 +
                  (int)Char.GetNumericValue(num_CPF[1]) * 10 +
                  (int)Char.GetNumericValue(num_CPF[2]) * 9 +
                  (int)Char.GetNumericValue(num_CPF[3]) * 8 +
                  (int)Char.GetNumericValue(num_CPF[4]) * 7 +
                  (int)Char.GetNumericValue(num_CPF[5]) * 6 +
                  (int)Char.GetNumericValue(num_CPF[6]) * 5 +
                  (int)Char.GetNumericValue(num_CPF[7]) * 4 +
                  (int)Char.GetNumericValue(num_CPF[8]) * 3 +
                  (int)Char.GetNumericValue(num_CPF[9]) * 2;
            digito2 = (soma * 10) % 11;
            if (digito2 == 10)
                digito2 = 0;
            if (digito1 == (int)Char.GetNumericValue(num_CPF[9]) && digito2 == (int)Char.GetNumericValue(num_CPF[10]))
            {
                return (String.Format("{0}{1}{2}.{3}{4}{5}.{6}{7}{8}-{9}{10}", num_CPF[0], num_CPF[1], num_CPF[2], num_CPF[3], num_CPF[4], num_CPF[5],
                        num_CPF[6], num_CPF[7], num_CPF[8], num_CPF[9], num_CPF[10]));
            }
            else
            {
                Console.WriteLine("O CPF informado é inválido!");
                return null;
            }
        }

        /* Função Adicionar
         * Responsável pela funcionalidade de adição de novos clientes no programa
         */
        static void Adicionar()
        {
            Cliente cliente = new Cliente();
            // Cabeçalho da opção Adicionar Cliente
            Console.WriteLine("\n\t\t\tAdição de um novo cliente\n");
            Console.WriteLine("===========================================================================\n");
            Console.Write("Nome Completo: ");
            cliente.nome = Console.ReadLine();
            while (cliente.cpf == null) 
            {
                Console.Write("CPF (Apenas números): ");
                cliente.cpf = Valida_CPF(Console.ReadLine());
            }
            //cliente.cpf = Console.ReadLine();
            Console.Write("E-mail: ");
            cliente.email = Console.ReadLine();
            ultimoCodigo = ultimoCodigo + 1;
            cliente.codigo = ultimoCodigo;
            clientes.Add(cliente); // adicionando o cliente a lista de Clientes
            Salvar();

            Console.WriteLine("\n===========================================================================\n" +
                "");
            Console.WriteLine("Cadastro finalizado com sucesso!!! \n\nAperte enter para retornar ao Menu Principal.");
            Console.ReadLine(); 
        }

        /* Função Listar
         * Responsável pela funcionalidade de listagem dos clientes cadastrados no programa
         */
        static void Listar()
        {
            if (clientes.Count > 0)
            {
                // Cabeçalho da opção Listar Clientes
                Console.WriteLine("\t\t\tListagem de clientes");
                Console.WriteLine("===========================================================================");
                foreach (Cliente cliente in clientes)
                {
                    Console.WriteLine(String.Format("\nCódigo: {0}", cliente.codigo));
                    Console.WriteLine(String.Format("Nome: {0}", cliente.nome));
                    Console.WriteLine(String.Format("CPF: {0}", cliente.cpf));
                    Console.WriteLine(String.Format("E-mail: {0}", cliente.email));
                }
            }
            else
            {
                Console.WriteLine("Ainda não há nenhum cliente cadastrado!");
            }
            Console.WriteLine("\n===========================================================================\n");
            Console.WriteLine("Aperte enter para retornar ao Menu Principal.");
            Console.ReadLine();
        }

        /* Função Excluir()
         * Responsável pela funcionalidade de exclusão de algum cliente com base no código do cliente
         */
        static void Excluir()
        {
            // Cabeçalho da opção Listar Clientes
            Console.WriteLine("\t\t\tExclusão de cliente");
            Console.WriteLine("===========================================================================");
            Console.Write("\nInforme o código do cliente a ser excluído: ");
            int.TryParse(Console.ReadLine(), out int codigoCliente);
            if (codigoCliente > 0 && codigoCliente < clientes.Count)
            {
                clientes.RemoveAt(codigoCliente - 1);
                Salvar();
            }
            else
            {
                Console.WriteLine("O código informado não pertence a nenhum cliente!");
            }
            Console.WriteLine("\n===========================================================================\n");
            Console.WriteLine("Aperte enter para retornar ao Menu Principal.");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            int opcao;
            Carregar(); // Carregando os dados dos clientes
            do
            {
                // Cabeçalho do programa que será exibido para o usuário
                Console.WriteLine("\n\t\tSeja bem vindo ao Sistema de Gestão de Clientes!!!\n");
                Console.WriteLine("===========================================================================");
                Console.WriteLine("\nDigite o número correspondente a operação que deseja realizar:");
                Console.WriteLine("1 - Listar clientes\n2 - Adicionar novo cliente\n3 - Excluir cliente\n4 - Sair");
                opcao = Entrada("\nOpção desejada: ");  // Chamando a função entrada
                Menu operacao = (Menu)opcao;    // Convertendo o valor numérico da opção no item do Menu
                Console.Clear(); // Limpando o Console
                // Direcionando o programa para a funcionalidade desejada pelo usuário
                switch (operacao)
                {
                    case Menu.LISTAR:
                        Listar();
                        break;
                    case Menu.ADICIONAR:
                        Adicionar();
                        break;
                    case Menu.EXCLUIR:
                        Excluir();
                        break;
                    case Menu.SAIR:
                        break;
                }
                Console.Clear();
            } while (opcao != 4);
        }
    }
}
