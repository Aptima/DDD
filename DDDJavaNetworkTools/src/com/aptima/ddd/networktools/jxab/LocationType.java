//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, v2.1-b02-fcs 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2009.05.25 at 05:05:58 PM EDT 
//

package com.aptima.ddd.networktools.jxab;

import java.io.Serializable;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;

/**
 * <p>
 * Java class for anonymous complex type.
 * 
 * <p>
 * The following schema fragment specifies the expected content contained within
 * this class.
 * 
 * <pre>
 * &lt;complexType&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base=&quot;{http://www.w3.org/2001/XMLSchema}anyType&quot;&gt;
 *       &lt;sequence&gt;
 *         &lt;element ref=&quot;{}X&quot;/&gt;
 *         &lt;element ref=&quot;{}Y&quot;/&gt;
 *         &lt;element ref=&quot;{}Z&quot;/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = { "x", "y", "z" })
@XmlRootElement(name = "LocationType")
public class LocationType implements Serializable {

	@XmlElement(name = "X")
	protected double x;

	@XmlElement(name = "Y")
	protected double y;

	@XmlElement(name = "Z")
	protected double z;
	public LocationType() {
		// TODO Auto-generated constructor stub
	}
	public LocationType(double x, double y, double z) {
		super();
		this.x = x;
		this.y = y;
		this.z = z;
	}

	/**
	 * Gets the value of the x property.
	 * 
	 */
	public double getX() {
		return x;
	}

	/**
	 * Gets the value of the y property.
	 * 
	 */
	public double getY() {
		return y;
	}

	/**
	 * Gets the value of the z property.
	 * 
	 */
	public double getZ() {
		return z;
	}

	/**
	 * Sets the value of the x property.
	 * 
	 */
	public void setX(double value) {
		this.x = value;
	}

	/**
	 * Sets the value of the y property.
	 * 
	 */
	public void setY(double value) {
		this.y = value;
	}

	/**
	 * Sets the value of the z property.
	 * 
	 */
	public void setZ(double value) {
		this.z = value;
	}
	
	public String toString() {
		return "( "+x+", "+y+", "+z+" )";
	}

}