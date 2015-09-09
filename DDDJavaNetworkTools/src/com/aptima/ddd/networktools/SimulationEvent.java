package com.aptima.ddd.networktools;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.xml.bind.annotation.XmlElementRef;

import com.aptima.ddd.networktools.jxab.Attribute;
import com.aptima.ddd.networktools.jxab.CustomAttribute;
import com.aptima.ddd.networktools.jxab.Parameter;
import com.aptima.ddd.networktools.jxab.Value;

public class SimulationEvent implements Serializable{

	private String eventType;
	public String msg;
	private Map<String, Parameter> names;

	@XmlElementRef(name = "Parameter", type = Parameter.class)
	public List<Parameter> parameters;

	public SimulationEvent() {
		this.setEventType("BaseEvent");
		parameters = Collections.synchronizedList(new ArrayList<Parameter>());
		names = Collections.synchronizedMap(new HashMap<String, Parameter>());
	}

	public SimulationEvent(SimulationEventTypes type) {
		this();
		this.setEventType(type.toString());
	}

	/**
	 * Intentionally Mutilated to avoid JXAB pick up as an element
	 * 
	 * @return the eventType
	 */
	public String _getEventType() {
		return eventType;
	}

	public void _setEventType(String type) {
		eventType = type;
	}

	public void addParameter(Parameter p) {
		addParameter(p.getName(), p.getValue());
	}

	public void addParameter(String name, Value value) {

		Parameter p = null;
		if (names.containsKey(name)) {
			p = names.get(name);
			p.setValue(value);
		} else {
			p = new Parameter();
			p.setName(name);
			p.setValue(value);
			names.put(name, p);
			parameters.add(p);
		}

	}

	public Parameter getParameter(String name) {
		// Check to see if the name->param map is empty
		if (names.size() == 0) {
			for (Parameter p : parameters) {
				names.put(p.getName(), p);
			}
		}
		return names.get(name);
	}

	public List<Parameter> getParameters() {
		return parameters;
	}

	/**
	 * @param eventType
	 *            the eventType to set
	 */
	public void setEventType(String eventType) {
		this.eventType = eventType;
	}

	public String toString() {
		return eventType;
	}
	
	public String toString(boolean verbose) {;
		if (verbose == false)
			return toString();
		
		String s = "";
		for (Parameter dv : getParameters()) {
			if (dv.getName().equals("Attributes")) {
				for (Attribute a : dv.getValue()
						.getAttributeCollectionType().getAttribute())
					s += a + "\n";
			} else if (dv.getName().equals("customAttribute")) {
				for (CustomAttribute a : dv.getValue()
						.getCustomAttributesType().getCustomAttribute())
					s += a + "\n";
			} else {
				s += dv + "\n";
			}
		}
		return s;
	}

	/*
	 * This code is from Value.java which is automatically generated by JXAB,
	 * but this code is code that I added to make it slightly easier to use. It
	 * is pasted here in case someone regenerates it and over the files.
	 */
	// Reflection Magic to get the non-null choice for this XML element
	// public Object get_Value() {
	// for (Field field : Value.class.getDeclaredFields()) {
	// try {
	// Object f = field.get(this);
	// if (!f.equals(null)) {
	// return f;
	// }
	// } catch (Exception e) {
	// // Meh- we tried
	// }
	// }
	//
	// return this;
	// }
	//
	// public String toString() {
	// return get_Value().toString();
	// }
}