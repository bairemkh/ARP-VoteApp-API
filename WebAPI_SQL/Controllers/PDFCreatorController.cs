using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using WebAPI_SQL.Services;
using WebAPI_SQL.Useful_Stuff;
using Wkhtmltopdf.NetCore;

namespace WebAPI_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFCreatorController : ControllerBase
    {
        private IGeneratePdf generate;

        public PDFCreatorController(IGeneratePdf generate)
        {
            this.generate = generate;
        }
        [HttpGet("GetPdf")]
        public string GetPdf()
        {
            try
            {
                var html = new StringBuilder();
                List<User> users = Useful_Methodes.GetAllUsers();
                html.Append(@"<html>
<head>
    <title>Last voting session result</title>
</head>
<body>
    <style>
       
        h1 {
            font-family: Cardo, Almarai;
            color: #BB2424;
        }
        
        table{
            align-content:center;
            align-items:center;
        }
        .styled-table {
            align-content:center;
            border-collapse: collapse;
            margin: 25px 0;
            font-size: 0.9em;
            font-family: sans-serif;
            min-width: 400px;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
        }

            .styled-table thead tr {
                background-color: #BB2424;
                color: #ffffff;
                text-align: left;
            }

            .styled-table th,
            .styled-table td {
                padding: 12px 15px;
            }

            .styled-table tbody tr {
                border-bottom: 1px solid #dddddd;
            }

                .styled-table tbody tr:nth-of-type(even) {
                    background-color: #f3f3f3;
                    
                }

                .styled-table tbody tr:last-of-type {
                    border-bottom: 2px solid #BB2424;
                }

                .styled-table tbody tr.active-row {
                    font-weight: bold;
                    color: #BB2424;
                }
                img{
                    align-content:center;
                    max-height:100px;
                    max-width:100px;
                }
                div{
                    justify-content:center;
                }
        .center {
            display: flex;
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }
        .typo {
            color: #BB2424;
            font-family:Algerian;
        }
    </style>
    <div align=""center"">
        <img src =""C:\Users\Bairemkh\Desktop\PFE\projet\Rep Vote Application\Rep Vote Application\Rep Vote Application.Android\Resources\drawable\Coat_of_arms_of_Tunisia.png""/>
     </div >
    <div>
        <h2 align=""center"" class=""typo"">مجلس نواب الشعب</h2>
    </div>
 <h1 align=""center"">Last voting session result</h1>");
                var votesession = Useful_Methodes.getLastSession();
                var law = Useful_Methodes.GetLaw(votesession.LawId);
                var result = Useful_Methodes.GetResult();
                html.AppendFormat(@"<h2 align=""center"">the voted law :{0}</h2>
        <h2 align=""center"">the vote is about :{1}</h2>
<br/>
<h3 align=""center"">the final vote is :{2}</h2>
            ", law.LawDesc, votesession.VoteSubject, result.FinalVote);
                html.Append(@"<table class=""styled-table"" align=""center"">
        <thead>
            <tr>
                <th align=""center"">Name</th>
                <th>Last Name</th>
                <th>Political Partie</th>
                <th>Voted yes</th>
                <th>Voted no</th>
                <th>Voted retained</th>
                <th>didn't vote</th>
            </tr>
        </thead>
        <tbody>");

                foreach (var user in users)
                {
                    html.AppendFormat(@"<tr>
                <td>{0}</td>
                <td>{1}</td>
                <td>{2}</td>                
                ", user.UserName, user.UserLastName, user.UserPoliticalParty);
                    var vote = Useful_Methodes.RetriveUserVote(user.UserId).Vote;
                    switch (vote.ToLower())
                    {
                        case "yes":
                            html.Append(@"<td>X</td>
                <td></td>
                <td></td>
                <td></td>
                </tr>");
                            break;
                        case "no":
                            html.Append(@"<td></td>
                <td>X</td>
                <td></td>
                <td></td>
                </tr>");
                            break;
                        case "retained":
                            html.Append(@"<td></td>
                <td></td>
                <td>X</td>
                <td></td> 
                </tr>");
                            break;
                        default:
                            html.Append(@"<td></td>
                <td></td>
                <td></td>
                <td>X</td> 
                </tr>");
                            break;
                    }
                }
                html.Append(@"</tbody>
                </table>
                </body>
                </html>");

                var pdf = generate.GetPDF(html.ToString());
                var PdfSteam = new MemoryStream();
                PdfSteam.Write(pdf, 0, pdf.Length);
                PdfSteam.Position = 0;
                var jsonContent = JsonConvert.SerializeObject(pdf);
                return jsonContent;
            }
            catch (WebException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
