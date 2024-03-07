﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult InsertCliente()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]       
        public ActionResult InsertCliente(Cliente cliente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string query = "INSERT INTO Cliente (Nominativo, IsAzienda, codiceFiscale, partitaIva) VALUES ( @Nominativo, @IsAzienda, @codiceFiscale, @partitaIva )";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nominativo", cliente.Nominativo);
                cmd.Parameters.AddWithValue("@IsAzienda", cliente.IsAzienda);
                cmd.Parameters.AddWithValue("@codiceFiscale", cliente.codiceFiscale ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@partitaIva", cliente.partitaIva ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Clienti()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Cliente> listaClienti = new List<Cliente>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM Cliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.idCliente = Convert.ToInt32(reader["idCliente"]);
                    cliente.Nominativo = reader["Nominativo"].ToString();
                    cliente.IsAzienda = Convert.ToBoolean(reader["IsAzienda"].ToString());
                    cliente.codiceFiscale = reader["codiceFiscale"].ToString();
                    cliente.partitaIva = reader["partitaIva"].ToString();
                    listaClienti.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return View(listaClienti);

        }
        
        public ActionResult InsertSpedizione()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult InsertSpedizione(Spedizione spedizione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string query = "INSERT INTO Spedizione (idCliente, codTracciamento, dataSpedizione, pesoSpedizione, cittaDestinazione, indirizzoDestinazione, nominativoDestinatario, costoSpedizione, dataConsegna) VALUES ( @idCliente, @codTracciamento, @dataSpedizione, @pesoSpedizione, @cittaDestinazione, @indirizzoDestinazione, @nominativoDestinatario, @costoSpedizione, @dataConsegna )";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idCliente", spedizione.idCliente);
                cmd.Parameters.AddWithValue("@codTracciamento", spedizione.CodTracciamento);
                cmd.Parameters.AddWithValue("@dataSpedizione", spedizione.DataSpedizione);
                cmd.Parameters.AddWithValue("@pesoSpedizione", spedizione.PesoSpedizione);
                cmd.Parameters.AddWithValue("@cittaDestinazione", spedizione.CittaDestinazione);
                cmd.Parameters.AddWithValue("@indirizzoDestinazione", spedizione.IndirizzoDestinazione);
                cmd.Parameters.AddWithValue("@nominativoDestinatario", spedizione.NominativoDestinatario);
                cmd.Parameters.AddWithValue("@costoSpedizione", spedizione.CostoSpedizione);
                cmd.Parameters.AddWithValue("@dataConsegna", spedizione.DataConsegna);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Spedizioni()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Spedizione> listaSpedizioni = new List<Spedizione>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM Spedizione";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spedizione spedizione = new Spedizione();
                    spedizione.IdSpedizione = Convert.ToInt32(reader["IdSpedizione"]);
                    spedizione.idCliente = Convert.ToInt32(reader["idCliente"]);
                    spedizione.CodTracciamento = reader["CodTracciamento"].ToString();
                    spedizione.DataSpedizione = Convert.ToDateTime(reader["DataSpedizione"]);
                    spedizione.PesoSpedizione = Convert.ToDecimal(reader["PesoSpedizione"]);
                    spedizione.CittaDestinazione = reader["CittaDestinazione"].ToString();
                    spedizione.IndirizzoDestinazione = reader["IndirizzoDestinazione"].ToString();
                    spedizione.NominativoDestinatario = reader["NominativoDestinatario"].ToString();
                    spedizione.CostoSpedizione = Convert.ToDecimal(reader["CostoSpedizione"]);
                    spedizione.DataConsegna = Convert.ToDateTime(reader["DataConsegna"]);
                    listaSpedizioni.Add(spedizione);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return View(listaSpedizioni);

        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]       
        public ActionResult Login(Utenti u)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
                try
                {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Utenti WHERE NomeUtente = @NomeUtente AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("NomeUtente", u.NomeUtente);
                cmd.Parameters.AddWithValue("Password", u.Password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(u.NomeUtente, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    conn.Close();
                    ViewBag.AuthError = "Autenticazione non riuscita";
                    return View();

                }
            }
            catch (Exception ex)
            {
                ViewBag.AuthError = "Errore durante l'autenticazione: " + ex.Message;
                // Qui potresti registrare l'eccezione in un file di log o fare altro con l'eccezione catturata
            }
            finally { conn.Close(); }

            return RedirectToAction("Index", "Home");

        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}