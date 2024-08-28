//using Novell.Directory.Ldap;
//using System;
//using System.Linq;

//namespace LdapApplication.Model
//{
//    public class LdapManager : IDisposable
//    {
//        private readonly string _ldapHost;
//        private readonly string _username;
//        private readonly int _ldapPort;
//        private readonly string _password;
//        private LdapConnection _connection;

//        public LdapManager(string ldapHost, int ldapPort, string username, string password)
//        {
//            _ldapHost = ldapHost;
//            _username = username;
//            _ldapPort = ldapPort;
//            _password = password;
//            Connect();
//        }

//        private void Connect()
//        {
//            try
//            {
//                _connection = new LdapConnection();
//                _connection.Connect(_ldapHost, _ldapPort);
//                _connection.Bind(_username, _password);

//                Console.WriteLine("Conexão e autenticação bem-sucedidas.");
//            }
//            catch (LdapException ex)
//            {
//                Console.WriteLine($"Error connecting to LDAP: {ex.Message}");
//                throw;
//            }
//        }

//        public void AddGroup(string identifier, string description)
//        {
//            try
//            {
//                var dn = $"cn={identifier},ou=Groups,dc=artigiani,dc=com";
//                var attributes = new LdapAttributeSet
//                {
//                    new LdapAttribute("objectClass", new[] {"groupOfNames", "top" }),
//                    new LdapAttribute("cn", identifier),
//                    new LdapAttribute("description", description),
//                    new LdapAttribute("member", _username)
//                };

//                var ldapEntry = new LdapEntry(dn, attributes);
//                _connection.Add(ldapEntry);
//            }
//            catch (LdapException ex)
//            {
//                Console.WriteLine($"Error adding group: {ex.Message}");
//                throw;
//            }
//        }

//        public void AddUser(string fullName, string login, string phone, string[] groups)
//        {
//            try
//            {
//                var dn = $"cn={login},ou=Users,dc=artigiani,dc=com";
//                var attributes = new LdapAttributeSet
//                {
//                    new LdapAttribute("objectClass", "inetOrgPerson"),
//                    new LdapAttribute("cn", fullName),
//                    new LdapAttribute("sn", login),
//                    new LdapAttribute("telephoneNumber", phone),
//                    new LdapAttribute("userPassword", "{SSHA}hashedpassword") // Example of a hashed password
//                };

//                var ldapEntry = new LdapEntry(dn, attributes);
//                _connection.Add(ldapEntry);

//                // Associar grupos ao usuário
//                foreach (var group in groups)
//                {
//                    var groupDn = $"cn={group},ou=Groups,dc=artigiani,dc=com";
//                    var groupEntry = _connection.Read(groupDn);
//                    var membersAttribute = groupEntry.GetAttribute("member");

//                    // Criar ou atualizar a lista de membros
//                    var currentMembers = membersAttribute != null ? membersAttribute.StringValueArray : new string[0];
//                    var newMembers = currentMembers.Append(dn).ToArray();

//                    // Criar a modificação de substituição
//                    var modification = new LdapModification(LdapModification.Replace, new LdapAttribute("member", newMembers));

//                    // Aplicar a modificação ao grupo
//                    _connection.Modify(groupDn, modification);
//                }
//            }
//            catch (LdapException ex)
//            {
//                Console.WriteLine($"Error adding user: {ex.Message}");
//                throw;
//            }
//        }

//        public void ModifyUserGroups(string login, string[] removeGroups, string[] addGroups)
//        {
//            try
//            {
//                var userDn = $"cn=Teste1,ou=Users,dc=artigiani,dc=com";

//                // Remover o usuário dos grupos especificados
//                foreach (var group in removeGroups)
//                {
//                    var groupDn = $"cn={group},ou=Groups,dc=artigiani,dc=com";
//                    var groupEntry = _connection.Read(groupDn);
//                    var membersAttribute = groupEntry.GetAttribute("member");

//                    if (membersAttribute != null)
//                    {
//                        var currentMembers = membersAttribute.StringValueArray;
//                        var newMembers = currentMembers.Where(m => m != userDn).ToArray();

//                        // Atualizar o atributo de membros com os usuários restantes
//                        var modification = new LdapModification(LdapModification.Replace, new LdapAttribute("member", newMembers));
//                        _connection.Modify(groupDn, modification);

//                        Console.WriteLine($"Removed user {login} from group {group}");
//                    }
//                    else
//                    {
//                        Console.WriteLine($"No 'member' attribute found for group {group}");
//                    }
//                }

//                // Adicionar o usuário aos grupos especificados
//                foreach (var group in addGroups)
//                {
//                    var groupDn = $"cn={group},ou=Groups,dc=artigiani,dc=com";
//                    var groupEntry = _connection.Read(groupDn);
//                    var membersAttribute = groupEntry.GetAttribute("member");

//                    var currentMembers = membersAttribute != null ? membersAttribute.StringValueArray : new string[0];
//                    var newMembers = currentMembers.Contains(userDn) ? currentMembers : currentMembers.Append(userDn).ToArray();

//                    var modification = new LdapModification(LdapModification.Replace, new LdapAttribute("member", newMembers));
//                    _connection.Modify(groupDn, modification);

//                    Console.WriteLine($"Added user {login} to group {group}");
//                }
//            }
//            catch (LdapException ex)
//            {
//                Console.WriteLine($"Error modifying user groups: {ex.Message}");
//                throw;
//            }
//        }




//        private string[] GetUserGroups(string userDn)
//        {
//            var groups = new List<string>();

//            try
//            {
//                // Procurar por todos os grupos e verificar se o usuário é membro
//                var searchFilter = $"(member={userDn})";
//                var searchResults = _connection.Search("ou=Groups,dc=example,dc=com", LdapConnection.ScopeSub, searchFilter, new[] { "dn" }, false);

//                while (searchResults.HasMore())
//                {
//                    var entry = searchResults.Next();
//                    var groupDn = entry.Dn;
//                    groups.Add(groupDn);
//                }
//            }
//            catch (LdapException ex)
//            {
//                Console.WriteLine($"Erro ao buscar grupos do usuário: {ex.Message}");
//                throw;
//            }

//            return groups.ToArray();
//        }


//        public void Dispose()
//        {
//            _connection?.Disconnect();
//        }
//    }
//}


using Novell.Directory.Ldap;
using System;
using System.Linq;

namespace LdapApplication.Model
{
    public class LdapManager : IDisposable
    {
        private readonly string _ldapHost;
        private readonly string _username;
        private readonly int _ldapPort;
        private readonly string _password;
        private LdapConnection _connection;

        public LdapManager(string ldapHost, int ldapPort, string username, string password)
        {
            _ldapHost = ldapHost;
            _username = username;
            _ldapPort = ldapPort;
            _password = password;
            Connect();
        }

        private void Connect()
        {
            try
            {
                _connection = new LdapConnection();
                _connection.Connect(_ldapHost, _ldapPort);
                _connection.Bind(_username, _password);

                Console.WriteLine("Conexão e autenticação bem-sucedidas.");
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Erro ao conectar ao LDAP: {ex.Message}");
                throw;
            }
        }

        public void AddGroup(string identifier, string description)
        {
            try
            {
                var dn = $"cn={identifier},ou=Groups,dc=artigiani,dc=com";
                var attributes = new LdapAttributeSet
                {
                    new LdapAttribute("objectClass", new[] {"groupOfNames", "top" }),
                    new LdapAttribute("cn", identifier),
                    new LdapAttribute("description", description),
                    new LdapAttribute("member", _username)
                };

                var ldapEntry = new LdapEntry(dn, attributes);
                _connection.Add(ldapEntry);
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Erro ao adicionar grupo: {ex.Message}");
                throw;
            }
        }

        public void AddUser(string fullName, string login, string phone, string[] groups)
        {
            try
            {
                var dn = $"cn={login},ou=Users,dc=artigiani,dc=com";
                var attributes = new LdapAttributeSet
                {
                    new LdapAttribute("objectClass", "inetOrgPerson"),
                    new LdapAttribute("cn", fullName),
                    new LdapAttribute("sn", login),
                    new LdapAttribute("telephoneNumber", phone),
                    new LdapAttribute("userPassword", "{SSHA}hashedpassword") // Exemplo de uma senha criptografada
                };

                var ldapEntry = new LdapEntry(dn, attributes);
                _connection.Add(ldapEntry);

                // Associar grupos ao usuário
                foreach (var group in groups)
                {
                    var groupDn = $"cn={group},ou=Groups,dc=artigiani,dc=com";
                    var groupEntry = _connection.Read(groupDn);
                    var membersAttribute = groupEntry.GetAttribute("member");

                    // Criar ou atualizar a lista de membros
                    var currentMembers = membersAttribute != null ? membersAttribute.StringValueArray : new string[0];
                    var newMembers = currentMembers.Append(dn).ToArray();

                    // Criar a modificação de substituição
                    var modification = new LdapModification(LdapModification.Replace, new LdapAttribute("member", newMembers));

                    // Aplicar a modificação ao grupo
                    _connection.Modify(groupDn, modification);
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Erro ao adicionar usuário: {ex.Message}");
                throw;
            }
        }

        public void ModifyUserGroups(string login, string[] removeGroups, string[] addGroups)
        {
            try
            {
                var userDn = $"cn=Teste1,ou=Users,dc=artigiani,dc=com";

                // Remover o usuário dos grupos especificados
                foreach (var group in removeGroups)
                {
                    var groupDn = $"cn={group},ou=Groups,dc=artigiani,dc=com";
                    var groupEntry = _connection.Read(groupDn);
                    var membersAttribute = groupEntry.GetAttribute("member");

                    if (membersAttribute != null)
                    {
                        var currentMembers = membersAttribute.StringValueArray;
                        var newMembers = currentMembers.Where(m => m != userDn).ToArray();

                        // Atualizar o atributo de membros com os usuários restantes
                        var modification = new LdapModification(LdapModification.Replace, new LdapAttribute("member", newMembers));
                        _connection.Modify(groupDn, modification);

                        Console.WriteLine($"Usuário {login} removido do grupo {group}");
                    }
                    else
                    {
                        Console.WriteLine($"Nenhum atributo 'member' encontrado para o grupo {group}");
                    }
                }

                // Adicionar o usuário aos grupos especificados
                foreach (var group in addGroups)
                {
                    var groupDn = $"cn={group},ou=Groups,dc=artigiani,dc=com";
                    var groupEntry = _connection.Read(groupDn);
                    var membersAttribute = groupEntry.GetAttribute("member");

                    var currentMembers = membersAttribute != null ? membersAttribute.StringValueArray : new string[0];
                    var newMembers = currentMembers.Contains(userDn) ? currentMembers : currentMembers.Append(userDn).ToArray();

                    var modification = new LdapModification(LdapModification.Replace, new LdapAttribute("member", newMembers));
                    _connection.Modify(groupDn, modification);

                    Console.WriteLine($"Usuário {login} adicionado ao grupo {group}");
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Erro ao modificar grupos de usuário: {ex.Message}");
                throw;
            }
        }

        private string[] GetUserGroups(string userDn)
        {
            var groups = new List<string>();

            try
            {
                // Procurar por todos os grupos e verificar se o usuário é membro
                var searchFilter = $"(member={userDn})";
                var searchResults = _connection.Search("ou=Groups,dc=example,dc=com", LdapConnection.ScopeSub, searchFilter, new[] { "dn" }, false);

                while (searchResults.HasMore())
                {
                    var entry = searchResults.Next();
                    var groupDn = entry.Dn;
                    groups.Add(groupDn);
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Erro ao buscar grupos do usuário: {ex.Message}");
                throw;
            }

            return groups.ToArray();
        }

        public void Dispose()
        {
            _connection?.Disconnect();
        }
    }
}
