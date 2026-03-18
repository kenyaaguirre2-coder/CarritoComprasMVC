// site.js - Lógica del lado del cliente para el carrito de compras
// Kenya Joseline Aguirre Leyva

// Función para mostrar los datos de sessionStorage en consola
function mostrarSessionStorage() {
    console.log('=== DATOS EN SESSION STORAGE ===');

    // Mostrar todas las claves en sessionStorage
    for (let i = 0; i < sessionStorage.length; i++) {
        const clave = sessionStorage.key(i);
        const valor = sessionStorage.getItem(clave);

        console.log(`${clave}:`, valor);

        // Intentar parsear si es JSON
        if (valor.startsWith('{') || valor.startsWith('[')) {
            try {
                const objeto = JSON.parse(valor);
                console.log(`  → Parseado:`, objeto);
            } catch (e) {
                // No es JSON válido
            }
        }
    }
    console.log('===============================');
}

// Función para guardar datos en sessionStorage
function guardarEnSessionStorage(clave, valor) {
    if (typeof valor === 'object') {
        sessionStorage.setItem(clave, JSON.stringify(valor));
    } else {
        sessionStorage.setItem(clave, valor);
    }
    console.log(`✅ Datos guardados: ${clave} =`, valor);
    mostrarSessionStorage();
}

// Función para limpiar sessionStorage
function limpiarSessionStorage() {
    sessionStorage.clear();
    console.log('🗑️ Session Storage limpiado');
    mostrarSessionStorage();
}

// Función para sincronizar datos del servidor con sessionStorage
function sincronizarConServidor() {
    console.log('🔄 Sincronizando con el servidor...');

    // Aquí puedes hacer peticiones AJAX para obtener datos del servidor
    // y guardarlos en sessionStorage

    // Ejemplo de cómo guardar datos del comprador si existen en la página
    const nombreCompradorElement = document.querySelector('.alert-info strong + span');
    if (nombreCompradorElement) {
        const nombreComprador = nombreCompradorElement.textContent;
        guardarEnSessionStorage('nombreComprador', nombreComprador);
    }

    // Guardar información de productos desde la tabla
    const productos = [];
    const filas = document.querySelectorAll('table tbody tr');

    filas.forEach(fila => {
        const celdas = fila.querySelectorAll('td');
        if (celdas.length >= 3) {
            productos.push({
                id: celdas[0].textContent,
                nombre: celdas[1].textContent,
                precio: celdas[2].textContent.replace('$', '')
            });
        }
    });

    if (productos.length > 0) {
        guardarEnSessionStorage('productosVisibles', productos);
    }

    // Guardar el total
    const totalElement = document.querySelector('h4 span');
    if (totalElement) {
        guardarEnSessionStorage('totalCarrito', totalElement.textContent);
    }
}

// Función para restaurar datos desde sessionStorage a la página
function restaurarDesdeSessionStorage() {
    console.log('🔄 Restaurando datos desde sessionStorage...');

    const nombreGuardado = sessionStorage.getItem('nombreComprador');
    if (nombreGuardado) {
        console.log('Nombre del comprador guardado:', nombreGuardado);
        // Aquí podrías actualizar la UI si es necesario
    }

    mostrarSessionStorage();
}

// Evento que se ejecuta cuando la página carga
document.addEventListener('DOMContentLoaded', function () {
    console.log('🚀 Página cargada - Iniciando site.js');

    // Mostrar el estado actual de sessionStorage
    mostrarSessionStorage();

    // Sincronizar datos del servidor con sessionStorage
    sincronizarConServidor();

    // Agregar botón para ver sessionStorage (solo en desarrollo)
    agregarBotonDepuracion();
});

// Función para agregar botón de depuración (solo visible en desarrollo)
function agregarBotonDepuracion() {
    // Verificar si ya existe el botón
    if (document.getElementById('btnVerSessionStorage')) return;

    // Crear botón flotante para depuración
    const boton = document.createElement('button');
    boton.id = 'btnVerSessionStorage';
    boton.innerHTML = '🔍 Ver Session Storage';
    boton.style.position = 'fixed';
    boton.style.bottom = '20px';
    boton.style.right = '20px';
    boton.style.zIndex = '9999';
    boton.style.backgroundColor = '#17a2b8';
    boton.style.color = 'white';
    boton.style.border = 'none';
    boton.style.borderRadius = '50px';
    boton.style.padding = '10px 20px';
    boton.style.cursor = 'pointer';
    boton.style.boxShadow = '0 2px 10px rgba(0,0,0,0.2)';

    boton.onclick = function () {
        mostrarSessionStorage();
        alert('Revisa la consola del navegador (F12) para ver los datos de Session Storage');
    };

    document.body.appendChild(boton);
}

// Función para guardar un producto en sessionStorage (cliente)
function guardarProductoLocal(nombre, precio) {
    let productosLocales = JSON.parse(sessionStorage.getItem('productosLocales') || '[]');

    productosLocales.push({
        id: Date.now(), // ID temporal basado en timestamp
        nombre: nombre,
        precio: precio,
        fecha: new Date().toLocaleString()
    });

    sessionStorage.setItem('productosLocales', JSON.stringify(productosLocales));
    console.log('✅ Producto guardado localmente:', { nombre, precio });
    mostrarSessionStorage();

    return productosLocales;
}

// Función para obtener productos locales
function obtenerProductosLocales() {
    return JSON.parse(sessionStorage.getItem('productosLocales') || '[]');
}

// Exportar funciones para uso global
window.carritoUtils = {
    mostrarSessionStorage,
    guardarEnSessionStorage,
    limpiarSessionStorage,
    sincronizarConServidor,
    restaurarDesdeSessionStorage,
    guardarProductoLocal,
    obtenerProductosLocales
};

// Inicializar
console.log('📦 site.js cargado correctamente');
console.log('🔧 Utilidades disponibles en window.carritoUtils');