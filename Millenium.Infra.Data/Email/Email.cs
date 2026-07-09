using Millenium.Domain.Entity;
using System;
using System.Net;
using System.Net.Mail;

namespace Millenium.Infra.Data.Email
{
    public class Email
    {
        public Email() { }

        public Email(Solicitacao solicitacao, string emailEnvio, string assunto, string mensagem)
        {
            EnviarEmail(solicitacao, emailEnvio, assunto, mensagem);
        }

        public Email(Usuario usuario, string novaSenha)
        {
            EnviarEmail(usuario, novaSenha);
        }

        public Email(string de, string deNome, string para, string assunto, string mensagem)
        {
            EnviarEmail(de, deNome, para, assunto, mensagem);
        }

        private void EnviarEmail(Solicitacao solicitacao, string emailEnvio, string assunto, string mensagem)
        {
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress("pesquisa@milleniumpesquisas.com.br");
                m.To.Add(new MailAddress(emailEnvio, "Solicitação de pesquisa realizada"));
                m.Subject = assunto;
                m.Body = mensagem;                    
                m.IsBodyHtml = true;
                sc.Host = "mail.milleniumpesquisas.com.br";
                sc.Port = 25;
                sc.Credentials = new NetworkCredential("pesquisa@milleniumpesquisas.com.br", "Fiesta@1991");
                sc.EnableSsl = false;
                sc.Send(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void EnviarEmail(Usuario usuario, string novaSenha)
        {
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress("suporte@milleniumpesquisas.com.br");
                m.To.Add(new MailAddress(usuario.Email, usuario.Nome));
                m.Subject = "Contato";
                m.Body =
                    " <br/> Usuário:  " + usuario.Login +
                    " <br/> Senha : " + novaSenha;
                m.IsBodyHtml = true;
                sc.Host = "mail.milleniumpesquisas.com.br";
                sc.Port = 25;
                sc.Credentials = new NetworkCredential("suporte@milleniumpesquisas.com.br", "Fiesta@1991");
                sc.EnableSsl = false;
                sc.Send(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void EnviarEmail(string de, string deNome, string para, string assunto, string mensagem)
        {
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress("pesquisa@milleniumpesquisas.com.br", deNome);
                m.ReplyToList.Add(new MailAddress(de, deNome));
                m.To.Add(new MailAddress(para));
                m.Subject = assunto;
                m.Body = mensagem;                    
                m.IsBodyHtml = true;
                sc.Host = "mail.milleniumpesquisas.com.br";
                sc.Port = 25;
                sc.Credentials = new NetworkCredential("pesquisa@milleniumpesquisas.com.br", "Fiesta@1991");
                sc.EnableSsl = false;
                sc.Send(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
