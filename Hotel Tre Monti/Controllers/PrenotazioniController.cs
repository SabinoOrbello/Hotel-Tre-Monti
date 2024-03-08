using Hotel_Tre_Monti.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel_Tre_Monti.Controllers
{
    public class PrenotazioniController : Controller
    {
        // GET: Prenotazioni
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["gestionale"].ConnectionString;
        public ActionResult Index()
        {
            List<Prenotazione> prenotazioni = new List<Prenotazione>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM Prenotazioni";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Prenotazione prenotazione = new Prenotazione
                        {
                            NumeroPrenotazione = (int)reader["NumeroPrenotazione"],
                            CodiceFiscaleCliente = reader["CodiceFiscaleCliente"].ToString(),
                            NumeroCamera = (int)reader["NumeroCamera"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivoAnno = (int)reader["NumeroProgressivoAnno"],
                            Anno = (int)reader["Anno"],
                            PeriodoDal = (DateTime)reader["PeriodoDal"],
                            PeriodoAl = (DateTime)reader["PeriodoAl"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            MezzaPensione = (bool)reader["MezzaPensione"],
                            PensioneCompleta = (bool)reader["PensioneCompleta"],
                            PernottamentoConColazione = (bool)reader["PernottamentoConColazione"]
                        };

                        prenotazioni.Add(prenotazione);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return View(prenotazioni);
        }


        public ActionResult VisualizzaCamere()
        {
            List<Camera> camereDisponibili = GetCamereDisponibili();

            ViewBag.Tariffe = GetTariffe();

            return View(camereDisponibili);
        }

        public ActionResult ConfermaPrenotazione()
        {

            return View();
        }

        public ActionResult Prenota()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Prenota(string codiceFiscaleCliente, int numeroCamera, DateTime? dataPrenotazione, int numeroProgressivoAnno, int anno, DateTime? periodoDal, DateTime? periodoAl, decimal caparraConfirmatoria, decimal tariffa, bool? mezzaPensione, bool? pensioneCompleta, bool? colazione)
        {
            // Esegui la logica di gestione della prenotazione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Esempio: Inserisci i dati della prenotazione nel database
                string query = "INSERT INTO Prenotazioni ( CodiceFiscaleCliente, NumeroCamera, DataPrenotazione, NumeroProgressivoAnno, Anno, PeriodoDal, PeriodoAl, CaparraConfirmatoria, Tariffa, MezzaPensione, PensioneCompleta, PernottamentoConColazione) " +
                               "VALUES (@CodiceFiscaleCliente, @NumeroCamera, @DataPrenotazione,@NumeroProgressivoAnno, @Anno, @PeriodoDal, @PeriodoAl, @CaparraConfirmatoria, @Tariffa, @MezzaPensione, @PensioneCompleta, @PernottamentoConColazione)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodiceFiscaleCliente", codiceFiscaleCliente);
                    command.Parameters.AddWithValue("@NumeroCamera", numeroCamera);
                    command.Parameters.AddWithValue("@DataPrenotazione", dataPrenotazione);
                    command.Parameters.AddWithValue("@NumeroProgressivoAnno", numeroProgressivoAnno);
                    command.Parameters.AddWithValue("@Anno", (int)anno);
                    command.Parameters.AddWithValue("@PeriodoDal", (object)periodoDal ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PeriodoAl", (object)periodoAl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CaparraConfirmatoria", caparraConfirmatoria);
                    command.Parameters.AddWithValue("@Tariffa", tariffa);
                    command.Parameters.AddWithValue("@MezzaPensione", (object)mezzaPensione ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PensioneCompleta", (object)pensioneCompleta ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PernottamentoConColazione", (object)colazione ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }

            // Reindirizza a una pagina di conferma o altro
            return RedirectToAction("ConfermaPrenotazione");
        }

        private List<Camera> GetCamereDisponibili()
        {
            List<Camera> camereDisponibili = new List<Camera>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Esempio: Recupera le camere disponibili dal database
                string query = "SELECT * FROM Camere";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Camera camera = new Camera
                            {
                                Numero = Convert.ToInt32(reader["Numero"]),
                                Descrizione = reader["Descrizione"].ToString(),
                                Tipologia = reader["Tipologia"].ToString()
                            };

                            camereDisponibili.Add(camera);
                        }
                    }
                }
            }

            return camereDisponibili;
        }

        private List<decimal> GetTariffe()
        {
            List<decimal> tariffe = new List<decimal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Esempio: Recupera le tariffe dalla tabella Prenotazioni
                string query = "SELECT DISTINCT Tariffa FROM Prenotazioni";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal tariffa = Convert.ToDecimal(reader["Tariffa"]);
                            tariffe.Add(tariffa);
                        }
                    }
                }
            }

            return tariffe;
        }

        public ActionResult Details(int numeroPrenotazione)
        {
            Prenotazione prenotazione = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                string sql = "SELECT * FROM Prenotazioni WHERE NumeroPrenotazione = @NumeroPrenotazione";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@NumeroPrenotazione", numeroPrenotazione);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())
                    {
                        prenotazione = new Prenotazione
                        {
                            NumeroPrenotazione = (int)reader["NumeroPrenotazione"],
                            CodiceFiscaleCliente = reader["CodiceFiscaleCliente"].ToString(),
                            NumeroCamera = (int)reader["NumeroCamera"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivoAnno = (int)reader["NumeroProgressivoAnno"],
                            Anno = (int)reader["Anno"],
                            PeriodoDal = (DateTime)reader["PeriodoDal"],
                            PeriodoAl = (DateTime)reader["PeriodoAl"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            MezzaPensione = (bool)reader["MezzaPensione"],
                            PensioneCompleta = (bool)reader["PensioneCompleta"],
                            PernottamentoConColazione = (bool)reader["PernottamentoConColazione"]
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

            if (prenotazione == null)
            {
                return RedirectToAction("Index");
            }


            return View(prenotazione);
        }

        public ActionResult Checkout(int numeroPrenotazione)
        {
            Prenotazione prenotazione = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                string sql = "SELECT * FROM Prenotazioni WHERE NumeroPrenotazione = @NumeroPrenotazione";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@NumeroPrenotazione", numeroPrenotazione);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())
                    {
                        prenotazione = new Prenotazione
                        {
                            NumeroPrenotazione = (int)reader["NumeroPrenotazione"],
                            CodiceFiscaleCliente = reader["CodiceFiscaleCliente"].ToString(),
                            NumeroCamera = (int)reader["NumeroCamera"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivoAnno = (int)reader["NumeroProgressivoAnno"],
                            Anno = (int)reader["Anno"],
                            PeriodoDal = (DateTime)reader["PeriodoDal"],
                            PeriodoAl = (DateTime)reader["PeriodoAl"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            MezzaPensione = (bool)reader["MezzaPensione"],
                            PensioneCompleta = (bool)reader["PensioneCompleta"],
                            PernottamentoConColazione = (bool)reader["PernottamentoConColazione"]
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

            if (prenotazione == null)
            {
                return RedirectToAction("Index");
            }


            return View(prenotazione);
        }

        public ActionResult AggiungiServizio()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AggiungiServizio(int numeroPrenotazione, DateTime dataServizio, string tipoServizio, int quantita, decimal prezzo)
        {
            Prenotazione prenotazione = GetPrenotazione(numeroPrenotazione);

            if (prenotazione != null)
            {
                ServizioAggiuntivo nuovoServizio = new ServizioAggiuntivo
                {
                    NumeroPrenotazione = numeroPrenotazione,
                    DataServizio = dataServizio,
                    TipoServizio = tipoServizio,
                    Quantita = quantita,
                    Prezzo = prezzo
                };

                prenotazione.ServiziAggiuntivi.Add(nuovoServizio);

                SalvaModifichePrenotazione(prenotazione);

                return RedirectToAction("Details", new { numeroPrenotazione });
            }
            else
            {
                return HttpNotFound(); // Prenotazione non trovata
            }
        }


        private Prenotazione GetPrenotazione(int numeroPrenotazione)
        {
            Prenotazione prenotazione = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Prenotazioni WHERE NumeroPrenotazione = @NumeroPrenotazione";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeroPrenotazione", numeroPrenotazione);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prenotazione = new Prenotazione
                            {
                                NumeroPrenotazione = Convert.ToInt32(reader["NumeroPrenotazione"]),
                                CodiceFiscaleCliente = reader["CodiceFiscaleCliente"].ToString(),
                                NumeroCamera = Convert.ToInt32(reader["NumeroCamera"]),
                                DataPrenotazione = Convert.ToDateTime(reader["DataPrenotazione"]),
                                // ... (altre proprietà)
                                ServiziAggiuntivi = new List<ServizioAggiuntivo>() // Inizializza la lista
                            };
                        }
                    }
                }
            }

            return prenotazione;
        }

        private void SalvaModifichePrenotazione(Prenotazione prenotazione)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Aggiorna la tabella Prenotazioni
                        string updatePrenotazioneQuery = @"
    UPDATE Prenotazioni
    SET CodiceFiscaleCliente = @CodiceFiscaleCliente,
        NumeroCamera = @NumeroCamera,
        DataPrenotazione = @DataPrenotazione,
        NumeroProgressivoAnno = @NumeroProgressivoAnno,
        Anno = @Anno,
        PeriodoDal = @PeriodoDal,
        PeriodoAl = @PeriodoAl,
        CaparraConfirmatoria = @CaparraConfirmatoria,
        Tariffa = @Tariffa,
        MezzaPensione = @MezzaPensione,
        PensioneCompleta = @PensioneCompleta,
        PernottamentoConColazione = @PernottamentoConColazione
    WHERE NumeroPrenotazione = @NumeroPrenotazione";

                        using (SqlCommand updatePrenotazioneCommand = new SqlCommand(updatePrenotazioneQuery, connection, transaction))
                        {
                            updatePrenotazioneCommand.Parameters.AddWithValue("@CodiceFiscaleCliente", prenotazione.CodiceFiscaleCliente);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@NumeroCamera", prenotazione.NumeroCamera);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@NumeroProgressivoAnno", prenotazione.NumeroProgressivoAnno);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@PeriodoDal", prenotazione.PeriodoDal);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@PeriodoAl", prenotazione.PeriodoAl);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@CaparraConfirmatoria", prenotazione.CaparraConfirmatoria);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@MezzaPensione", prenotazione.MezzaPensione);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@PensioneCompleta", prenotazione.PensioneCompleta);
                            updatePrenotazioneCommand.Parameters.AddWithValue("@PernottamentoConColazione", prenotazione.PernottamentoConColazione);


                            updatePrenotazioneCommand.ExecuteNonQuery();
                        }




                        // Inserisci i nuovi servizi aggiuntivi
                        string insertServiziQuery = @"
                    INSERT INTO ServiziAggiuntivi (NumeroPrenotazione, DataServizio, TipoServizio, Quantita, Prezzo)
                    VALUES (@NumeroPrenotazione, @DataServizio, @TipoServizio, @Quantita, @Prezzo)";

                        foreach (var servizio in prenotazione.ServiziAggiuntivi)
                        {
                            using (SqlCommand insertServizioCommand = new SqlCommand(insertServiziQuery, connection, transaction))
                            {
                                insertServizioCommand.Parameters.AddWithValue("@NumeroPrenotazione", prenotazione.NumeroPrenotazione);
                                insertServizioCommand.Parameters.AddWithValue("@DataServizio", servizio.DataServizio);
                                insertServizioCommand.Parameters.AddWithValue("@TipoServizio", servizio.TipoServizio);
                                insertServizioCommand.Parameters.AddWithValue("@Quantita", servizio.Quantita);
                                insertServizioCommand.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);

                                insertServizioCommand.ExecuteNonQuery();
                            }
                        }

                        // Committa la transazione
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Gestisci eventuali errori e fai il rollback della transazione in caso di fallimento
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }






    }
}