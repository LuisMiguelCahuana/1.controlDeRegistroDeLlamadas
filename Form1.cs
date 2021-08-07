using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.controlDeRegistroDeLlamadas
{
    public partial class frmLlamada : Form
    {
        // Declaracion de variables Globales
        string tipo;
        string horario;
        int minutos;
        double costoMinuto;
        double costoLlamada;

        double mayorMonto;
        string horarioMayor;
        string tipoMayor;

        // Contadores y acumuladores
        int cLlamadas;
        double aLocNac, aLocInt, aMovNac, aMovInt;

        private void tHora_Tick(object sender, EventArgs e)
        {
            // Mostrar la hora
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Capturando los datos
            horario = cboHorario.Text;
            minutos = int.Parse(txtMinutos.Text);

            // Determinar el costo por minuto
            asignaCostoxMinuto();

            // Determinar el costo por llamada
            asignaCostoxLlamada();

            // Imprimir el registro de llamadas
            imprimirRegistro();

            lvEstadisticas.Items.Clear();
        }

        private void btnEstadisticas_Click(object sender, EventArgs e)
        {
            // Enviar los datos a la lista de estadisticas
            imprimirEstadisticas();
        }
        // Metodo que permite asignar el costo por minuto segun el tipo de llamada
        void asignaCostoxMinuto()
        {
            // Capturando el tipo de llamada desde el cuadro combinado
            tipo = cboTipo.Text;

            // Asignado el costo por minuto segun el tipo de llamada
            switch (tipo)
            {
                case "Local Nacional": costoMinuto = 0.20; break;
                case "Local Internacional": costoMinuto = 0.50; break;
                case "Movil Nacional": costoMinuto = 1.20; break;
                case "Movil Internacional": costoMinuto = 2.20; break;
            }
        }
        // Metodo que permite asignar por llamada segun el horario
        void asignaCostoxLlamada()
        {
            // Variables locales
            double importe = costoMinuto * minutos;
            double descuento = 0;

            // Asignando el descuento segun el horario
            switch (horario)
            {
                case "Diurno (07:00-13:00)": descuento = importe * 0.3; break;
                case "Tarde (13:00-19:00)": descuento = importe * 0.2; break;
                case "Noche (19:00-23:00)": descuento = importe * 0.1; break;
                case "Madrugada (23:00-07:00": descuento = importe * 0.3; break;
            }
            costoLlamada = importe - descuento;
        }

        // Metodo que permite imprimir los valores en la lista de registro
        void imprimirRegistro()
        {
            ListViewItem fila = new ListViewItem(tipo);
            fila.SubItems.Add(horario);
            fila.SubItems.Add(minutos.ToString());
            fila.SubItems.Add(costoMinuto.ToString("0.00"));
            fila.SubItems.Add(costoLlamada.ToString("0.00"));
            lvRegistro.Items.Add(fila);
        }
        // Metodo que permite mostrar los valores Globales para la estadistica
        void imprimirEstadisticas()
        {
            // Contar el numero de llamadas entre 10 y 30 minutos
            numeroLlamadas();

            // Monto acumulado del costo por llamada por tipo
            costoAcumuladoxTipo();

            // Mayor costo por llamada, que tipo y horario
            mayorMontoLlamada();

            // Enviando los resultados a la lista de estadisticas
            lvEstadisticas.Items.Clear();
            string[] elementosFila = new string[2];
            ListViewItem row;

            elementosFila[0] = "Numero de llamadas entre 10 y 30 minutos";
            elementosFila[1] = cLlamadas.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Local Nacional";
            elementosFila[1] = aLocNac.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Local Internacional";
            elementosFila[1] = aLocInt.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Movil Nacional";
            elementosFila[1] = aMovNac.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Movil Internacional";
            elementosFila[1] = aMovInt.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Mayor monto de llamada";
            elementosFila[1] = mayorMonto.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Tipo de llamada con mayor monto";
            elementosFila[1] = tipoMayor;
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Horario con mayor monto";
            elementosFila[1] = horarioMayor;
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);
        }

        // Determinar el numero de llamadas entre 10  y 30 minutos
        void numeroLlamadas()
        {
            // Inicializar el contador de llamadas
            cLlamadas = 0;

            // Recorremos por todo el registro de la lista
            for(int i=0; i<lvRegistro.Items.Count; i++)
            {
                // Capturamos los minutos
                int minutos = int.Parse(lvRegistro.Items[i].SubItems[2].Text);
                // Comparamos si los minutos se encuentran en el rango de 10 y 30
                if (minutos >= 10 && minutos <= 30) cLlamadas++;
            }
        }

        // Metodo que determina el total acumulado del costo por llamada
        void costoAcumuladoxTipo()
        {
            // Inicializar las variables acumuladoras en cero
            aLocNac = 0; aLocInt = 0; aMovNac = 0; aMovInt = 0;

            // Recorremos todos los registros
            for(int i=0; i<lvRegistro.Items.Count; i++)
            {
                // Capturando el tipo de llamada
                string t = lvRegistro.Items[i].SubItems[0].Text;

                // Condicionar el tipo de llamada para realizar la acumulacion
                if (t == "Local Nacional")
                    aLocNac += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                else if (t == "Local Internacional")
                    aLocInt += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                else if (t == "Movil Nacional")
                    aMovNac += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                else if (t == "Movil Internacional")
                    aMovInt += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
            }
        }

        // Metodo que determinar el mayor monto de llamada
        void mayorMontoLlamada()
        {
            // Inicializar la variable local posicion
            int posicion = 0;

            // Inicializar la variable mayor con el primer costo de los registros
            mayorMonto = double.Parse(lvRegistro.Items[0].SubItems[4].Text);

            // Recorrer por todos los registros
            for(int i=0; i<lvRegistro.Items.Count; i++)
            {
                /* Si uno de los costos es mayor que el valor asignado a la variable
                   mayor entonces hemos encontrado el mayor de los elementos */
                if(double.Parse(lvRegistro.Items[i].SubItems[4].Text) > mayorMonto)
                {
                    mayorMonto = double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                    posicion = i;
                }
            }
            tipoMayor = lvRegistro.Items[posicion].SubItems[0].Text;
            horarioMayor = lvRegistro.Items[posicion].SubItems[1].Text;
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Asignar el costo por minuto
            asignaCostoxMinuto();
            lblCostos.Text = costoMinuto.ToString("S/0.00");
        }

        public frmLlamada()
        {
            InitializeComponent();
            tHora.Enabled = true;
        }

        private void frmLlamada_Load(object sender, EventArgs e)
        {
            // Mostrar la fecha actual
            lblFecha.Text = DateTime.Now.ToShortDateString();
        }
    }
}
