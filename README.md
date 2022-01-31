<h1>Storage Service &nbsp; &nbsp;<img src="https://user-images.githubusercontent.com/74969971/151826919-808c23e1-3691-47a9-ab5e-c4dcca3f7007.png" style="width:5%;"></h1> 
<p>Het inkrijgen en opslaan van xml-documenten.</p>

<h2>Description</h2>
Inkrijgen van een xml-document door de document broker via de IN Queue. </br>
Vervolgens het XSD-validatie (enkel en alleen xml document inkrijgen), het document moet een ticket, documenttype en binary hebben.

Daarna moet het bepalen of dat archief tijdelijk is of permanent.

Aansluitend het opslaan van het document op een fileserver. Als dat gelukt is moet er een antwoord gegenereerd worden op de OUT Queue.


<h2>Getting Started</h2>
<h3>Dependencies</h3>
<ul>
  <li><h4>Packages</h4></li>
    <ul>
      <li>Docker.DotNet</li>
      <li>RabbitMQ.Client</li>
      <li>System.Data.SqlClient</li>
    </ul>
  <li><h4>Frameworks</h4></li>
    <ul>
      <li>Microsoft.NETCore.App</li>
    </ul>
</ul>
<h3>Installing</h3>
Download Docker before starting <a href="https://www.docker.com/products/docker-desktop">here</a>.  
</br>
</br>
<a href="https://www.docker.com/products/docker-desktop"><img src="https://user-images.githubusercontent.com/74969971/151821895-d072e45f-34d8-4001-9dcc-2ebc18feff00.png" style="width:5%;">  </a>
</br>
</br> 


Het downloaden van het programma is via Github. (Code -> download zip -> extract file)
Er zijn geen wijzigingen die gebracht moeten worden in de files/folders.
<h3>Executing program</h3>
Het runnen van de app: -> Execute \OpslagServiceG3\bin\Debug\App.exe
*how to use RabbitMQ*
<h3>Help</h3>
Any advise for common problems or issues.

command to run if program contains helper info
<h2>Authors</h2>
Contributors names and contact info
</br>
</br>
<a href="mailto:mohamad.bahaa.alahmad1@student.ehb.be" style="none">Mohamad Bahaa Alahmad</a>
</br>
<a href="mailto:mouise.bashir@student.ehb.be">Mouise Bashir</a>
</br>
<a href="ine.debast@student.ehb.be">Ine Debast</a>
</br>
<a href="mailto:marialine.iselona.mboyo@student.ehb.be">Marialine Iselona Mboyo</a>
</br>
</br>

https://www.erasmushogeschool.be/nl

<h4>Initial Release</h4>
27 januari 2022


<h2>Acknowledgments</h2>
Thank you for the help Wannes and Maryam

