# 🛠️ LDAP XML Processor

[![C#](https://img.shields.io/badge/C%23-9E8CFF?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/) 
[![OpenLDAP](https://img.shields.io/badge/OpenLDAP-3776AB?style=for-the-badge&logo=openldap&logoColor=white)](https://www.openldap.org/)

> **C# application to manage LDAP entries using XML files. The application reads and processes XML to perform CRUD operations on an LDAP directory, ensuring data validation and proper segregation of users and groups.**

## 🚀 Features

- **XML Parsing with XPath**: Efficient extraction and manipulation of XML data.
- **LDAP Integration**: Seamless interaction with OpenLDAP using native LDAP classes.
- **Data Validation**: Ensures text-only fields for names and logins, and numeric-only fields for phone numbers using regular expressions.
- **Container Segregation**: Users and groups are managed in distinct LDAP containers.

## 📂 Project Structure

```plaintext
├── src/
│   ├── Model/
│   │   ├── Validador.cs         # XML validation logic
│   │   └── LdapManager.cs       # LDAP operations management
│   ├── Controller/
│   │   └── LdapController.cs    # Handles requests and operations
│   └── Program.cs               # Main entry point of the application
├── data/
    ├── AddGrupo1.xml            # Sample XML for adding a group
    ├── AddGrupo2.xml            # Sample XML for adding a group
    ├── AddGrupo3.xml            # Sample XML for adding a group
    ├── AddUsuario1.xml          # Sample XML for adding a user
    ├── ModifyUsuario1.xml       # Sample XML for modifying a user
```


🛠️ Getting Started
Prerequisites
.NET 6.0 SDK or higher
An operational LDAP server (e.g., OpenLDAP)

Access to Git for cloning the repository
Installation

Clone the repository:
git clone https://github.com/your-username/ldap-xml-processor.git
cd ldap-xml-processor

Install dependencies:
dotnet restore

Running the Application
dotnet run --project src/Program.cs
🤖 Usage
Add/Modify LDAP Entries:

Place your XML files in the data/ directory.
Run the application to process and apply the changes to your LDAP server.
XML Format: Ensure your XML files follow the required structure for users and groups.

✨ Acknowledgements
Novell.Directory.Ldap.NETStandard for LDAP operations.
OpenLDAP for the LDAP server.

🌐 Connect
GitHub: @guiartigiani

LinkedIn: [Your Name](https://www.linkedin.com/in/guilherme-artigiani/)

Crafted with 💻 by Guilherme Artigiani.
