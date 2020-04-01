/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package duypn.controllers;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import duypn.classes.ApplicationContants;
import duypn.classes.HttpHelper;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.http.HttpResponse;
import java.util.LinkedHashMap;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import org.json.JSONObject;

/**
 *
 * @author yiyil
 */
public class SearchScheduleController extends HttpServlet {

    /**
     * Processes requests for both HTTP <code>GET</code> and <code>POST</code>
     * methods.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html;charset=UTF-8");
        String url = ApplicationContants.ERROR;
        try {
            String date = request.getParameter("DateSearch");
            HttpHelper h = new HttpHelper();
            HttpSession session = request.getSession();
            ObjectMapper mapper = new ObjectMapper();
            
            if(date == null){
                request.setAttribute("ERROR", "Search cannot be blank");
            }
            else {
                JSONObject account = (JSONObject)session.getAttribute("Account");
                
                // Send request
                String urlGet = ApplicationContants.URL_GET_SCHEDULE_ONWEEK+"?studentId="+account.getString("username")+"&date="+date;
                HttpResponse<String> res = h.sendGet(urlGet);
                JSONObject j = new JSONObject(res.body());
                
                LinkedHashMap<String, Object> result = mapper.readValue(j.getJSONObject("Data").toString(), new TypeReference<LinkedHashMap<String, Object>>() {
                });
                
                request.setAttribute("lastSearchValue", date);
                request.setAttribute("result", result);
                
                url = ApplicationContants.PAGE_ATTENDANCE;
            }
            
        } catch (Exception e) {
        } finally {
            request.getRequestDispatcher(url).forward(request, response);
        }
    }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
