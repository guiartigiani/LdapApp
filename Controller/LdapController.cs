using System.Xml.Linq;
using LdapApplication.Model;

namespace LdapApplication.Controller
{
    public class LDAPController
    {
        private readonly LdapManager _ldapManager;

        public LDAPController(LdapManager ldapManager)
        {
            _ldapManager = ldapManager;
        }

        public void ProcessAddGroupXml(string filePath)
        {
            try
            {
                // Carregar o arquivo XML
                var xmlDoc = XDocument.Load(filePath);

                // Obter o elemento raiz
                var element = xmlDoc.Root;
                if (element == null)
                {
                    throw new InvalidOperationException("O documento XML está vazio ou não possui um elemento raiz.");
                }

                // Extrair os valores
                var identifier = GetValue(element, "Identificador");
                var description = GetValue(element, "Descricao");

                // Validar valores
                if (!Validator.ValidateLogin(identifier))
                {
                    throw new ArgumentException("O identificador do grupo é inválido.");
                }

                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentException("A descrição do grupo não pode estar vazia.");
                }

                // Adicionar o grupo
                _ldapManager.AddGroup(identifier, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o XML: {ex.Message}");
                // Log ou manipule a exceção conforme necessário
            }
        }

        public void ProcessAddUserXml(string filePath)
        {
            try
            {
                var xmlDoc = XDocument.Load(filePath);
                var element = xmlDoc.Root;
                if (element == null)
                {
                    throw new InvalidOperationException("O documento XML está vazio ou não possui um elemento raiz.");
                }

                var fullName = GetValue(element, "Nome Completo");
                var login = GetValue(element, "Login");
                var phone = GetValue(element, "Telefone");
                var groups = GetValues(element, "Grupo");

                //Validar valores
                if (!Validator.ValidateFullName(fullName))
                {
                    throw new ArgumentException("O nome completo do usuário é inválido.");
                }

                if (!Validator.ValidateLogin(login))
                {
                    throw new ArgumentException("O login do usuário é inválido.");
                }

                if (!Validator.ValidatePhone(phone))
                {
                    throw new ArgumentException("O telefone do usuário é inválido.");
                }

                // Adicionar o usuário
                _ldapManager.AddUser(fullName, login, phone, groups);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o XML: {ex.Message}");
                // Log ou manipule a exceção conforme necessário
            }
        }

        public void ProcessModifyUserXml(string filePath)
        {
            try
            {
                var xmlDoc = XDocument.Load(filePath);
                var element = xmlDoc.Root;
                if (element == null)
                {
                    throw new InvalidOperationException("O documento XML está vazio ou não possui um elemento raiz.");
                }

                var login = element.Element("association")?.Value;
                var removeGroups = GetValuesFromModify(element, "remove-value");
                var addGroups = GetValuesFromModify(element, "add-value");

                if (login == null || !Validator.ValidateLogin(login))
                {
                    throw new ArgumentException("O login do usuário deve ser especificado e válido no XML de modificação.");
                }

                // Modificar os grupos do usuário
                _ldapManager.ModifyUserGroups(login, removeGroups, addGroups);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o XML: {ex.Message}");
                // Log ou manipule a exceção conforme necessário
            }
        }

        private string GetValue(XElement element, string attrName)
        {
            var addAttrElement = element.Elements("add-attr")
                                        .FirstOrDefault(e => e.Attribute("attr-name")?.Value == attrName);
            if (addAttrElement == null)
            {
                Console.WriteLine($"Elemento com attr-name='{attrName}' não encontrado.");
                return null;
            }

            var valueElement = addAttrElement.Element("value");
            if (valueElement == null)
            {
                Console.WriteLine($"Elemento <value> não encontrado dentro de <add-attr> com attr-name='{attrName}'.");
                return null;
            }

            return valueElement.Value.Trim();
        }

        private string[] GetValues(XElement element, string attrName)
        {
            return element.Elements("add-attr")
                          .FirstOrDefault(e => e.Attribute("attr-name")?.Value == attrName)
                          ?.Elements("value")
                          .Select(v => v.Value)
                          .ToArray();
        }

        private string[] GetValuesFromModify(XElement element, string valueType)
        {
            return element.Elements("modify-attr")
                          .Elements(valueType)
                          .Elements("value")
                          .Select(v => v.Value)
                          .ToArray();
        }
    }
}
