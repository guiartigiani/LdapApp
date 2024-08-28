using System;
using LdapApplication.Controller;
using LdapApplication.Model;

class Program
{
    static void Main()
    {
        string ldapPath = "localhost"; // Porta padrão para LDAP é 389
        string username = "cn=admin,dc=artigiani,dc=com"; // Atualize com o DN do usuário administrador
        int ldapPort = 389;
        string password = "123"; //

        string group1 = "C:\\Users\\guilh\\OneDrive\\Documentos\\Guilherme Arquivos\\DesafioOpenConsult\\LdapApp\\Arquivos XML\\AddGrupo1.xml";
        string group2 = "C:\\Users\\guilh\\OneDrive\\Documentos\\Guilherme Arquivos\\DesafioOpenConsult\\LdapApp\\Arquivos XML\\AddGrupo2.xml";
        string group3 = "C:\\Users\\guilh\\OneDrive\\Documentos\\Guilherme Arquivos\\DesafioOpenConsult\\LdapApp\\Arquivos XML\\AddGrupo3.xml";
        string user1 = "C:\\Users\\guilh\\OneDrive\\Documentos\\Guilherme Arquivos\\DesafioOpenConsult\\LdapApp\\Arquivos XML\\AddUsuario1.xml";
        string modifyUser = "C:\\Users\\guilh\\OneDrive\\Documentos\\Guilherme Arquivos\\DesafioOpenConsult\\LdapApp\\Arquivos XML\\ModifyUsuario.xml";

        LdapManager ldapModel = new LdapManager(ldapPath, ldapPort, username, password);
        LDAPController ldapController = new LDAPController(ldapModel);

        bool continueRunning = true;

        while (continueRunning)
        {
            Console.Clear();
            Console.WriteLine("Escolha uma operação:");
            Console.WriteLine("1. Adicionar Grupo 1");
            Console.WriteLine("2. Adicionar Grupo 2");
            Console.WriteLine("3. Adicionar Grupo 3");
            Console.WriteLine("4. Adicionar Usuário");
            Console.WriteLine("5. Modificar Usuário");
            Console.WriteLine("0. Sair");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ldapController.ProcessAddGroupXml(group1);
                    Console.WriteLine("Grupo 1 adicionado.");
                    break;

                case "2":
                    ldapController.ProcessAddGroupXml(group2);
                    Console.WriteLine("Grupo 2 adicionado.");
                    break;

                case "3":
                    ldapController.ProcessAddGroupXml(group3);
                    Console.WriteLine("Grupo 3 adicionado.");
                    break;

                case "4":
                    ldapController.ProcessAddUserXml(user1);
                    Console.WriteLine("Usuário adicionado.");
                    break;

                case "5":
                    ldapController.ProcessModifyUserXml(modifyUser);
                    Console.WriteLine("Usuário modificado.");
                    break;

                case "0":
                    continueRunning = false;
                    Console.WriteLine("Saindo...");
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            if (continueRunning)
            {
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
                Console.ReadKey();
            }
        }
    }
}
