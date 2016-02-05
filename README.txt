Aplicatia reprezinta un client de Google Drive in limbajul C#.

1. Conectarea la contul real se face prin intermediul butonului de "Login"
	-in textBox-ul din dreapta butonului Login vor aparea informatiile referitoare la User (Id, nume, E-mail si tara)
2. Dupa apasarea butonului "Login" vor aparea restul de elemente.
3. Listarea continutului contului se va face prin intermediul butonului "List files", iar continutul va aparea in listBox-ul de dedesubt. La doubleclick pe element se va deschide continutul acestuia(daca exista). Butonul "Back" face posibila parcurgerea listei cu un parinte in spate.
4. In partea din dreapta a listbox-ului ce contine elementele din cont, se afla un textBox in care vor fi afisate informatii despre elementul selectat in momentul curent in listBox
5. Butonul "Download" face posibila descarcarea elementului selectat in momentul curent in listBox. Va trebui specificat locul de pe disc unde se va salva fisierul.
6. Butonul "Upload" face posibila incarcarea unui element selectat de dumneavoastra prin intermediul unui OpenFileDialog. In urma selectarii elementului, se va transmite calea acestuia la functia de upload si se va face copierea bit cu bit.
7. Pentru a sterge un element este indeajuns sa va pozitionati pe un element din listBox iar la apasarea butonului "Delete" acesta va fi sters.
8. Butonul "Copy" va efectua copierea elementului selectat din cardul listBox-ului. Copierea se va face in acelasi folder in care se gaseste si fisierul original, singura diferenta dintre cele doua fiind trailer-ul "COPIE" de la sfarsitul numelui. Pentru a muta acest fisier din folderul curent se va folosi butonul "Move" (va trebui specificat, in textBox-ul din dreptul sau, numele noului folder in care se doreste mutarea).
9. Pentru a share-ui un element, se va apasa butonul de share dupa ce a fost completat campul aferent acestuia cu e-mailul user-ului cu care se doreste a fi share-uit.

	Aplicatia estre structurata pe cele 3 nivele:
		-Data Layer reprezinta nivelul la care se stocheaza datele.
		-Business Layer este nivelul ce contine toate metodele si are acces la datele din DataLayer.
		-Prezentation Layer este nivelul ce contine formele, aici se face interactiunea cu user-ul.

Pentru realizarea acestui proiect am folosit tutorialele de pe site-ul:
	https://developers.google.com/drive/v2/reference/