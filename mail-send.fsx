#r "lib/MailKit.2.7.0/lib/netstandard2.0/MailKit.dll"
#r "lib/MimeKit.2.8.0/lib/netstandard2.0/MimeKit.dll"
#r "lib/Portable.BouncyCastle.1.8.5/lib/netstandard2.0/BouncyCastle.Crypto.dll"
#r "lib/FSharp.Data.3.3.3/lib/netstandard2.0/FSharp.Data.dll"

open MailKit
open MimeKit
open FSharp.Data

type ConnCon = JsonProvider<"./connection-provider.json">

let config_row = System.IO.File.ReadAllText(@"connection.json")

let config = ConnCon.Parse config_row

let msg = new MimeMessage()
let fromAddress = MailboxAddress("up", config.From)
let toAddress = MailboxAddress("hos", config.To)

let content = """
こんにちは

無能な上司を持つと不幸ですね。

かしこ
"""

let mutable builder = new BodyBuilder()
builder.TextBody <- content

msg.From.Add fromAddress
msg.To.Add toAddress
msg.Subject <- "はときいん"
msg.Body <- builder.ToMessageBody()

let host = config.Server
let port: int = config.Port

let sendmail = 
    use client = new Net.Smtp.SmtpClient()
    client.Connect(host, port)
    client.Authenticate(config.User, config.Pass)
    client.Send(msg)
    client.Disconnect(true)

