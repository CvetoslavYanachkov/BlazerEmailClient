# 📧 Blazor Email Client

This **Blazor Server** application allows users to **send and receive emails** using **IMAP and SMTP protocols**. The app enables users to **configure email settings**, send messages, and fetch emails from an IMAP server.

## 🚀 Features

- ✅ **Configure IMAP & SMTP settings** dynamically
- ✅ **Send emails** via SMTP
- ✅ **Fetch emails** from an IMAP server
- ✅ **View email content with formatted HTML**
- ✅ **Support for attachments & inline images**
- ✅ **User-friendly UI with Bootstrap**
- ✅ **Blazor Interactive Server Rendering**
- ✅ **IMAP & SMTP protocols implemented without any third-party libraries**

---

## ⚙️ **Setup & Installation**

### 1️⃣ **Extract the BlazorEmailClient Files**

Download the project archive and extract it to your preferred location:

1. Locate the downloaded `BlazorEmailClient.zip` file.
2. Right-click and select **Extract All...**.
3. Choose a destination folder and click **Extract**.
4. Navigate to the extracted `BlazorEmailClient` folder.
5. Navigate to the `EmailClientApp` folder.

### 2️⃣ **Install Dependencies**

Ensure you have **.NET 7+** installed. If not, [download .NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet).

```sh
dotnet restore
```

### 3️⃣ **Configure Email Settings**

You can either:

- Enter details manually in the **Configure Email** section of the UI.
- OR edit the `appsettings.json` file:

```json
"EmailSettings": {
   "ImapServer": "imap.gmail.com",
   "ImapPort": 993,
   "SmtpServer": "smtp.gmail.com",
   "SmtpPort": 465,
   "Email": "example@gmail.com"
}
```

---

## ▶️ **Run the Application**

```sh
dotnet run
```

Then, open a web browser and navigate to:

```
http://localhost:5233
```

---

## 📩 **Sending an Email**

1. Go to /send-email in the UI.
2. Enter:
   - **Recipient Email**
   - **Subject**
   - **Message Body**
3. Click **Send** and wait for a confirmation.

---

## 📥 **Fetching Latest Emails**

1. Go to /fetch-email in the UI.
2. The app will connect to the **IMAP server** and retrieve emails.
3. Emails will be displayed with:
   - ✅ **Sender**
   - ✅ **Subject**
   - ✅ **Body**
   - ✅ **Inline Images & Attachments**

---

## 🔹 **IMAP & SMTP Commands Used**

### 🔹 **Fetching Latest Emails**

```plaintext
A1 LOGIN user@example.com password
A2 SELECT INBOX
A3 FETCH <LATEST_UID> (RFC822)
A5 LOGOUT
```

### 🔹 **Sending an Email (SMTP)**

1. Connect to the SMTP server.
2. Authentication of the user.
3. Send `MAIL FROM`, `RCPT TO`, and `DATA`.
4. Send the email body.
5. End with `QUIT`.

---

## 🛠️ **Built With**

- 🟢 **Blazor Server** - UI framework
- 🟢 **C# & .NET 8+**
- 🟢 **Bootstrap** - Styling
- 🟢 **IMAP & SMTP protocols implemented without any third-party libraries**

---

## ⚠️ **Troubleshooting**

### **🔹 IMAP Authentication Issues**

- Enable **IMAP Access** in Gmail (Settings > Forwarding & POP/IMAP).
- Use **App Passwords** instead of your regular password.
- If using Gmail, allow **Less Secure Apps** or enable **OAuth**.

### **🔹 SMTP Issues**

- Check if your SMTP port is **465 (SSL) or 587 (TLS)**.
- If using **Gmail SMTP**, App Passwords are required.

---

## 📩 **Contact**

For questions or issues, reach out via  email at [**cyanachkov@gmail.com**](mailto\:cyanachkov@gmail.com).

---

🔥 **Now you're ready to send & receive emails with Blazor!** 🚀

